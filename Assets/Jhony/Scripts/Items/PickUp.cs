using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickup : MonoBehaviour
{
    public float VerticalBobFrequency = 1f;
    public float BobbingAmount = 1f;
    public float RotatingSpeed = 360f;
    public AudioClip PickupSfx;
    public GameObject PickupVfxPrefab;

    public Rigidbody PickupRigidbody { get; private set; }

    protected Collider m_Collider;
    protected Vector3 m_StartPosition;
    protected bool m_HasPlayedFeedback;

    protected virtual void Start()
    {
        PickupRigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        PickupRigidbody.isKinematic = true;
        m_Collider.isTrigger = true;

        m_StartPosition = transform.position;
    }

    protected virtual void Update()
    {
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * VerticalBobFrequency) * 0.5f) + 0.5f) * BobbingAmount;
        transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
        transform.Rotate(Vector3.up, RotatingSpeed * Time.deltaTime, Space.Self);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            OnPicked(player);
        }
    }

    protected virtual void OnPicked(Player player)
    {
        PlayPickupFeedback();
    }

    public void PlayPickupFeedback()
    {
        if (m_HasPlayedFeedback) return;

        if (PickupSfx)
        {
            AudioSource.PlayClipAtPoint(PickupSfx, transform.position);
        }

        if (PickupVfxPrefab)
        {
            Instantiate(PickupVfxPrefab, transform.position, Quaternion.identity);
        }

        m_HasPlayedFeedback = true;
    }
}