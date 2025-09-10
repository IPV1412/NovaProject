using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public GameObject WeaponRoot;
    public Transform WeaponMuzzle;
    public Bullet BulletPrefab;

    [Header("Shoot Settings")]
    public float DelayBetweenShots = 0.5f;
    public float BulletSpreadAngle = 0f;
    public int BulletsPerShot = 1;
    [Range(0f, 2f)] public float RecoilForce = 1f;

    [Header("Aiming")]
    [Range(0f, 1f)] public float AimZoomRatio = 1f;
    public Vector3 AimOffset;

    [Header("Ammo")]
    public int MaxAmmo = 30;
    public float AmmoReloadRate = 10f;
    public float AmmoReloadDelay = 1f;
    public bool AutomaticReload = true;

    [Header("VFX & SFX")]
    public GameObject MuzzleFlashPrefab;
    public AudioClip ShootSfx;

    public GameObject Owner { get; set; }

    AudioSource m_AudioSource;
    float m_CurrentAmmo;
    float m_LastTimeShot = Mathf.NegativeInfinity;
    float m_LastMuzzlePosTime;

    public float CurrentAmmoRatio { get; private set; }
    public bool IsWeaponActive { get; private set; }
    public bool IsReloading { get; private set; }

    void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_CurrentAmmo = MaxAmmo;
    }

    void Update()
    {
        UpdateAmmo();
    }

    void UpdateAmmo()
    {
        if (AutomaticReload && Time.time > m_LastTimeShot + AmmoReloadDelay && m_CurrentAmmo < MaxAmmo)
        {
            m_CurrentAmmo += AmmoReloadRate * Time.deltaTime;
            m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo, 0f, MaxAmmo);
        }

        CurrentAmmoRatio = m_CurrentAmmo / MaxAmmo;
    }

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);
        IsWeaponActive = show;
    }

    public void StartReloadAnimation()
    {
        IsReloading = true;
        m_CurrentAmmo = MaxAmmo;
        IsReloading = false;
    }

    public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        if (inputDown && Time.time > m_LastTimeShot + DelayBetweenShots && m_CurrentAmmo >= 1f)
        {
            Shoot();
            return true;
        }

        return false;
    }

    void Shoot()
    {
        for (int i = 0; i < BulletsPerShot; i++)
        {
            Vector3 shotDir = GetShotDirectionWithinSpread(WeaponMuzzle);
            var projectile = Instantiate(BulletPrefab, WeaponMuzzle.position, WeaponMuzzle.rotation);
            projectile.owner = gameObject;

        }

        if (MuzzleFlashPrefab)
        {
            var flash = Instantiate(MuzzleFlashPrefab, WeaponMuzzle.position, WeaponMuzzle.rotation, WeaponMuzzle);
            Destroy(flash, 2f);
        }

        if (ShootSfx)
        {
            m_AudioSource.PlayOneShot(ShootSfx);
        }

        m_CurrentAmmo -= 1f;
        m_LastTimeShot = Time.time;
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadRatio = BulletSpreadAngle / 180f;
        return Vector3.Slerp(shootTransform.forward, Random.insideUnitSphere, spreadRatio);
    }
}
