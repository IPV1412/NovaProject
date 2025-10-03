using UnityEngine;

public class MaxHealthPickup : Pickup
{
    public float extraHealth = 10f;

    protected override void OnPicked(Player player)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.MaxHealth += extraHealth;
            playerHealth.CurrentHealth += extraHealth;
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}