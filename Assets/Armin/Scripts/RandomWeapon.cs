using UnityEngine;
using System.Collections;

public class RandomWeapon : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    [SerializeField] private WeaponController defaultWeapon;
    [SerializeField] private WeaponController secondWeapon;
    [SerializeField] private WeaponController thirdWeapon;
    
    [Header("Visuals")]
    [SerializeField] private Renderer visualRenderer;
    [SerializeField] private Collider visualCollider;
    
    private bool isActive = true;
    private WeaponsManager currentWeaponManager;
    private Coroutine revertCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        
        if (other.CompareTag("Player"))
        {
            WeaponsManager weaponManager = other.GetComponent<WeaponsManager>();
            if (weaponManager == null) return;
            
            currentWeaponManager = weaponManager;
            
            WeaponController randomWeapon = GetRandomWeaponExcludingDefault();
            if (randomWeapon != null)
            {
                SwapToTemporaryWeapon(randomWeapon);
                DisablePickup();
            }
        }
    }
    
    private WeaponController GetRandomWeaponExcludingDefault()
    {
        System.Collections.Generic.List<WeaponController> availableWeapons = new System.Collections.Generic.List<WeaponController>();
        
        if (secondWeapon != null) availableWeapons.Add(secondWeapon);
        if (thirdWeapon != null) availableWeapons.Add(thirdWeapon);
        
        if (availableWeapons.Count > 0)
        {
            int randomIndex = Random.Range(0, availableWeapons.Count);
            return availableWeapons[randomIndex];
        }
        
        return null;
    }
    
    private void SwapToTemporaryWeapon(WeaponController newWeaponPrefab)
    {
        if (currentWeaponManager == null) return;
        
        WeaponController currentWeapon = currentWeaponManager.GetWeapon();
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        WeaponController newWeapon = Instantiate(newWeaponPrefab, currentWeaponManager.WeaponParentSocket);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.Owner = currentWeaponManager.gameObject;
        newWeapon.ShowWeapon(true);
        
        Debug.Log($"Temporary weapon equipped: {newWeaponPrefab.name}");

        if (revertCoroutine != null)
            StopCoroutine(revertCoroutine);
    
        revertCoroutine = StartCoroutine(RevertToDefaultWeaponAfterDelay(30f));
    }
    
    private IEnumerator RevertToDefaultWeaponAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RevertToDefaultWeapon();
    }
    
    private void RevertToDefaultWeapon()
    {
        if (currentWeaponManager == null || defaultWeapon == null) return;
        
        WeaponController currentWeapon = currentWeaponManager.GetWeapon();
        if (currentWeapon != null)
        {
            Transform weaponParent = currentWeapon.transform.parent;
            Vector3 localPos = currentWeapon.transform.localPosition;
            Quaternion localRot = currentWeapon.transform.localRotation;
            
            Destroy(currentWeapon.gameObject);
            
            WeaponController defaultWeaponInstance = Instantiate(defaultWeapon, weaponParent);
            defaultWeaponInstance.transform.localPosition = localPos;
            defaultWeaponInstance.transform.localRotation = localRot;
            defaultWeaponInstance.Owner = currentWeaponManager.gameObject;
            defaultWeaponInstance.ShowWeapon(true);
            
            Debug.Log("Reverted to default weapon");
        }
        
        currentWeaponManager = null;
        EnablePickup();
    }
    
    private void DisablePickup()
    {
        isActive = false;
        
        if (visualRenderer != null)
            visualRenderer.enabled = false;
            
        if (visualCollider != null)
            visualCollider.enabled = false;
    }
    
    private void EnablePickup()
    {
        isActive = true;
        
        if (visualRenderer != null)
            visualRenderer.enabled = true;
            
        if (visualCollider != null)
            visualCollider.enabled = true;
    }
    
    private void OnDestroy()
    {
        if (revertCoroutine != null)
            StopCoroutine(revertCoroutine);
    }
    
    private void OnValidate()
    {
        if (visualRenderer == null)
            visualRenderer = GetComponent<Renderer>();
            
        if (visualCollider == null)
            visualCollider = GetComponent<Collider>();
    }
}