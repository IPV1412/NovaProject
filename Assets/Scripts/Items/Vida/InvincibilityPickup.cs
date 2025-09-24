using UnityEngine;
using System.Collections;

public class InvincibilityPickup : Pickup
{
    public float invincibleDuration = 5f;

    protected override void OnPicked(Player player)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            StartCoroutine(GrantInvincibility(playerHealth));
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }

    private IEnumerator GrantInvincibility(Health playerHealth)
    {
        playerHealth.Invincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        playerHealth.Invincible = false;
    }
}

