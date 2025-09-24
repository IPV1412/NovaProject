using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Cámara y Movimiento")]
    public Camera PlayerCamera;
    public AudioSource AudioSource;
    public float LookSensitivity = 1f;
    public float WebglLookSensitivityMultiplier = 0.25f;
    public bool InvertYAxis = false;
    public bool InvertXAxis = false;

    public float RotationSpeed = 200f;
    public float AimingRotationMultiplier = 0.4f;
    public float GravityDownForce = 20f;
    public float JumpForce = 9f;

    [Header("Velocidades")]
    public float MaxSpeedOnGround = 10f;
    public float MaxSpeedCrouchedRatio = 0.5f;
    public float MaxSpeedInAir = 10f;
    public float AccelerationSpeedInAir = 25f;
    public float SprintSpeedModifier = 2f;
    public float MovementSharpnessOnGround = 15;

    [Header("Sokects Camaras")]
    public float CameraHeightRatio = 0.9f;
    public float CapsuleHeightStanding = 1.8f;
    public float CapsuleHeightCrouching = 0.9f;
    public float CrouchingSharpness = 10f;
    public LayerMask GroundCheckLayers = -1;
    public float GroundCheckDistance = 0.05f;
    public float KillHeight = -50f;

    [Header("Audios")]
    public float FootstepSfxFrequency = 1f;
    public float FootstepSfxFrequencyWhileSprinting = 1f;
    public AudioClip FootstepSfx;
    public AudioClip JumpSfx;
    public AudioClip LandSfx;
    public AudioClip FallDamageSfx;

    [Header("Daño por Caída")]
    public bool RecievesFallDamage;
    public float MinSpeedForFallDamage = 10f;
    public float MaxSpeedForFallDamage = 30f;
    public float FallDamageAtMinSpeed = 10f;
    public float FallDamageAtMaxSpeed = 50f;

    public Vector3 CharacterVelocity { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsCrouching { get; private set; }

    public bool IsAiming => Input.GetButton("Fire2");
    public float RotationMultiplier => IsAiming ? AimingRotationMultiplier : 1f;

    CharacterController m_Controller;
    Health m_Health;
    WeaponsManager m_WeaponsManager;

    Vector3 m_GroundNormal;
    Vector3 m_LatestImpactSpeed;
    float m_LastTimeJumped;
    float m_CameraVerticalAngle;
    float m_FootstepDistanceCounter;
    float m_TargetCharacterHeight;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_Controller = GetComponent<CharacterController>();
        m_Health = GetComponent<Health>();
        m_WeaponsManager = GetComponent<WeaponsManager>();

        m_Health.OnDie += () => IsDead = true;

        m_Controller.enableOverlapRecovery = true;
        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);
    }

    void Update()
    {
        if (IsDead) return;

        if (transform.position.y < KillHeight)
            m_Health.Kill();

        HasJumpedThisFrame = false;

        bool wasGrounded = IsGrounded;
        GroundCheck();

        if (IsGrounded && !wasGrounded)
        {
            float fallSpeed = -Mathf.Min(CharacterVelocity.y, m_LatestImpactSpeed.y);
            float fallSpeedRatio = (fallSpeed - MinSpeedForFallDamage) / (MaxSpeedForFallDamage - MinSpeedForFallDamage);

            if (RecievesFallDamage && fallSpeedRatio > 0f)
            {
                float dmg = Mathf.Lerp(FallDamageAtMinSpeed, FallDamageAtMaxSpeed, fallSpeedRatio);
                m_Health.TakeDamage(dmg, null);
                AudioSource.PlayOneShot(FallDamageSfx);
            }
            else
            {
                AudioSource.PlayOneShot(LandSfx);
            }
        }
        UpdateCharacterHeight(false);
        HandleCharacterMovement();
    }

    void GroundCheck()
    {
        float checkDistance = IsGrounded ? (m_Controller.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;
        IsGrounded = false;
        m_GroundNormal = Vector3.up;

        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height),
                m_Controller.radius, Vector3.down, out RaycastHit hit, checkDistance, GroundCheckLayers,
                QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;
                if (Vector3.Dot(hit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    IsGrounded = true;
                    if (hit.distance > m_Controller.skinWidth)
                        m_Controller.Move(Vector3.down * hit.distance);
                }
            }
        }
    }

    void HandleCharacterMovement()
    {
        transform.Rotate(0f, GetHorizontalLook() * RotationSpeed * RotationMultiplier, 0f);
        m_CameraVerticalAngle += GetVerticalLook() * RotationSpeed * RotationMultiplier;
        m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);
        PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0f, 0f);

        bool isSprinting = Input.GetButton("Sprint");
        if (isSprinting)
            isSprinting = SetCrouchingState(false, false);

        float speedMod = isSprinting ? SprintSpeedModifier : 1f;
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);
        Vector3 moveInput = transform.TransformVector(inputDir);

        if (IsGrounded)
        {
            Vector3 targetVelocity = moveInput * MaxSpeedOnGround * speedMod;
            if (IsCrouching) targetVelocity *= MaxSpeedCrouchedRatio;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;

            CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, MovementSharpnessOnGround * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && SetCrouchingState(false, false))
            {
                CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                CharacterVelocity += Vector3.up * JumpForce;
                AudioSource.PlayOneShot(JumpSfx);
                m_LastTimeJumped = Time.time;
                HasJumpedThisFrame = true;
                IsGrounded = false;
                m_GroundNormal = Vector3.up;
            }

            float stepFreq = isSprinting ? FootstepSfxFrequencyWhileSprinting : FootstepSfxFrequency;
            if (m_FootstepDistanceCounter >= 1f / stepFreq)
            {
                m_FootstepDistanceCounter = 0f;
                AudioSource.PlayOneShot(FootstepSfx);
            }

            m_FootstepDistanceCounter += CharacterVelocity.magnitude * Time.deltaTime;
        }
        else
        {
            CharacterVelocity += moveInput * AccelerationSpeedInAir * Time.deltaTime;
            float y = CharacterVelocity.y;
            Vector3 horizontal = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
            horizontal = Vector3.ClampMagnitude(horizontal, MaxSpeedInAir * speedMod);
            CharacterVelocity = horizontal + Vector3.up * y;
            CharacterVelocity += Vector3.down * GravityDownForce * Time.deltaTime;
        }

        Vector3 capsuleBottom = GetCapsuleBottomHemisphere();
        Vector3 capsuleTop = GetCapsuleTopHemisphere(m_Controller.height);
        m_Controller.Move(CharacterVelocity * Time.deltaTime);

        m_LatestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottom, capsuleTop, m_Controller.radius,
            CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime,
            -1, QueryTriggerInteraction.Ignore))
        {
            m_LatestImpactSpeed = CharacterVelocity;
            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
        }
    }

    Vector3 GetCapsuleBottomHemisphere() => transform.position + transform.up * m_Controller.radius;
    Vector3 GetCapsuleTopHemisphere(float height) => transform.position + transform.up * (height - m_Controller.radius);
    bool IsNormalUnderSlopeLimit(Vector3 normal) => Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;

    Vector3 GetDirectionReorientedOnSlope(Vector3 dir, Vector3 slopeNormal)
    {
        Vector3 right = Vector3.Cross(dir, transform.up);
        return Vector3.Cross(slopeNormal, right).normalized;
    }

    float GetHorizontalLook()
    {
        float input = Input.GetAxisRaw("Mouse X");
        if (InvertXAxis) input *= -1;
        return ApplyLookSensitivity(input);
    }

    float GetVerticalLook()
    {
        float input = Input.GetAxisRaw("Mouse Y");
        if (InvertYAxis) input *= -1;
        return ApplyLookSensitivity(input);
    }

    float ApplyLookSensitivity(float input)
    {
        float i = input * LookSensitivity;
#if UNITY_WEBGL
        i *= WebglLookSensitivityMultiplier;
#endif
        return i * 0.01f;
    }

    bool SetCrouchingState(bool crouch, bool ignoreObstructions)
    {
        if (!crouch && !ignoreObstructions)
        {
            Collider[] overlaps = Physics.OverlapCapsule(
                GetCapsuleBottomHemisphere(),
                GetCapsuleTopHemisphere(CapsuleHeightStanding),
                m_Controller.radius, -1, QueryTriggerInteraction.Ignore);

            foreach (Collider c in overlaps)
                if (c != m_Controller) return false;
        }

        m_TargetCharacterHeight = crouch ? CapsuleHeightCrouching : CapsuleHeightStanding;
        IsCrouching = crouch;
        return true;
    }

    void UpdateCharacterHeight(bool force)
    {
        if (force)
        {
            m_Controller.height = m_TargetCharacterHeight;
            m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
            PlayerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * CameraHeightRatio;
        }
        else if (m_Controller.height != m_TargetCharacterHeight)
        {
            m_Controller.height = Mathf.Lerp(m_Controller.height, m_TargetCharacterHeight, CrouchingSharpness * Time.deltaTime);
            m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
            PlayerCamera.transform.localPosition = Vector3.Lerp(PlayerCamera.transform.localPosition,
                Vector3.up * m_TargetCharacterHeight * CameraHeightRatio, CrouchingSharpness * Time.deltaTime);
        }
    }
}
