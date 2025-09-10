using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [Header("References")]
    public Camera WeaponCamera;
    public Transform WeaponParentSocket;
    public Transform DefaultWeaponPosition;
    public Transform AimingWeaponPosition;

    [Header("Weapon Settings")]
    public WeaponController startingWeaponPrefab;
    public float AimingAnimationSpeed = 10f;
    public float WeaponFovMultiplier = 1f;

    WeaponController weaponInstance;
    Player player;

    Vector3 m_WeaponLocalPos;
    Vector3 m_CurrentVelocity;
    Vector3 m_RecoilOffset;

    void Start()
    {
        player = GetComponent<Player>();

        if (startingWeaponPrefab != null)
        {
            weaponInstance = Instantiate(startingWeaponPrefab, WeaponParentSocket);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;
            weaponInstance.Owner = gameObject;
            weaponInstance.ShowWeapon(true);
        }

        if (player != null && player.PlayerCamera != null && WeaponCamera != null)
        {
            WeaponCamera.fieldOfView = player.PlayerCamera.fieldOfView * WeaponFovMultiplier;
        }
    }

    void Update()
    {
        if (weaponInstance == null || player == null || player.IsDead)
            return;

        if (weaponInstance.IsReloading)
            return;

        bool aimHeld = Input.GetButton("Fire2");
        bool fireDown = Input.GetButtonDown("Fire1");
        bool fireHeld = Input.GetButton("Fire1");
        bool fireReleased = Input.GetButtonUp("Fire1");

        if (!weaponInstance.AutomaticReload && Input.GetKeyDown(KeyCode.R) && weaponInstance.CurrentAmmoRatio < 1f)
        {
            weaponInstance.StartReloadAnimation();
            return;
        }

        weaponInstance.HandleShootInputs(fireDown, fireHeld, fireReleased);

        if (fireDown)
        {
            m_RecoilOffset += Vector3.back * weaponInstance.RecoilForce;
        }

        UpdateAiming(aimHeld);
    }

    void LateUpdate()
    {
        if (weaponInstance == null) return;

        WeaponParentSocket.localPosition = m_WeaponLocalPos + m_RecoilOffset;
        m_RecoilOffset = Vector3.Lerp(m_RecoilOffset, Vector3.zero, Time.deltaTime * 10f);
    }

    void UpdateAiming(bool isAiming)
    {
        if (weaponInstance == null) return;

        Vector3 targetPos = isAiming
            ? AimingWeaponPosition.localPosition + weaponInstance.AimOffset
            : DefaultWeaponPosition.localPosition;

        m_WeaponLocalPos = Vector3.Lerp(m_WeaponLocalPos, targetPos, AimingAnimationSpeed * Time.deltaTime);

        if (player != null && player.PlayerCamera != null)
        {
            float targetFov = isAiming
                ? weaponInstance.AimZoomRatio * player.RotationSpeed
                : player.RotationSpeed;

            player.PlayerCamera.fieldOfView = Mathf.Lerp(player.PlayerCamera.fieldOfView, targetFov, AimingAnimationSpeed * Time.deltaTime);
        }
    }

    public WeaponController GetWeapon()
    {
        return weaponInstance;
    }
}
