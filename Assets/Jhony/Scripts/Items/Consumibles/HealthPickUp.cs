using UnityEngine.Serialization;

public class HealthPickUp : Pickup
{
    public float healAmount;

    protected override void OnPicked(Player player)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth && playerHealth.CanPickup())
        {
            playerHealth.Heal(healAmount);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    } 
}
