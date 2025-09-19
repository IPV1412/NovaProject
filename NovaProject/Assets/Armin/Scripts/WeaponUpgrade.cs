using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponUpgrade : MonoBehaviour
{
    private WeaponsManager weaponManager;
    private WeaponController weapon;
    
    public enum UpgradeOptions { Default, FireRate, Ammo, ClipSize, Damage }
    public UpgradeOptions currentState;
    
    private float originalDelayBetweenShots;
    private int originalMaxAmmo;
    private int originalBulletsPerShot;
    private float originalAmmoReloadRate;
    private float originalAmmoReloadDelay;
    
    private bool upgradeActive = false;
    private Coroutine revertCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !upgradeActive)
        {
            weaponManager = other.GetComponent<WeaponsManager>();
            if (weaponManager == null) return;
            
            weapon = weaponManager.GetWeapon();
            if (weapon == null) return;
            
            StoreOriginalValues();
            currentState = (UpgradeOptions)Random.Range(1, 5);
            
            ApplyUpgrade(currentState);
            DisablePickupVisuals();
            
            if (revertCoroutine != null)
                StopCoroutine(revertCoroutine);
            
            revertCoroutine = StartCoroutine(RevertAfterDelay(10f));
        }
    }
    
    void StoreOriginalValues()
    {
        originalDelayBetweenShots = weapon.DelayBetweenShots;
        originalMaxAmmo = weapon.MaxAmmo;
        originalBulletsPerShot = weapon.BulletsPerShot;
        originalAmmoReloadRate = weapon.AmmoReloadRate;
        originalAmmoReloadDelay = weapon.AmmoReloadDelay;
    }
    
    void RevertToOriginalValues()
    {
        if (weapon == null) return;
        
        weapon.DelayBetweenShots = originalDelayBetweenShots;
        weapon.MaxAmmo = originalMaxAmmo;
        weapon.BulletsPerShot = originalBulletsPerShot;
        weapon.AmmoReloadRate = originalAmmoReloadRate;
        weapon.AmmoReloadDelay = originalAmmoReloadDelay;
        
        Destroy(gameObject);
    }
    
    IEnumerator RevertAfterDelay(float delay)
    {
        upgradeActive = true;
        yield return new WaitForSeconds(delay);
        RevertToOriginalValues();
        upgradeActive = false;
    }
    
    void DisablePickupVisuals()
    {
        Collider collider = GetComponent<Collider>();
        Renderer renderer = GetComponent<Renderer>();
        
        if (collider != null) collider.enabled = false;
        if (renderer != null) renderer.enabled = false;
    }
    
    void ApplyUpgrade(UpgradeOptions upgrade)
    {
        switch (upgrade)
        {
            case UpgradeOptions.FireRate:
                weapon.DelayBetweenShots = Mathf.Max(0.1f, weapon.DelayBetweenShots * 0.7f);
                break;
            case UpgradeOptions.Ammo:
                weapon.MaxAmmo = Mathf.Min(999, weapon.MaxAmmo + 20);
                break;
            case UpgradeOptions.ClipSize:
                weapon.BulletsPerShot = Mathf.Min(10, weapon.BulletsPerShot + 1);
                break;
            case UpgradeOptions.Damage:
                // Agregar daño para despues
                break;
            default:
                weapon.MaxAmmo = 30;
                weapon.AmmoReloadRate = 10f;
                weapon.DelayBetweenShots = 0.5f;
                weapon.BulletsPerShot = 1;
                weapon.AmmoReloadDelay = 1f;
                break;
        }
    }
    
    void OnDestroy()
    {
        if (revertCoroutine != null)
            StopCoroutine(revertCoroutine);
    }
}