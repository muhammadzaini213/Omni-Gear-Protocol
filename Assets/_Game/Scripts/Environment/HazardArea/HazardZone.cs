using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [Tooltip("Use case is when player in toxin area or some")]
    [SerializeField] private int areaDamage = 20;
    [Tooltip("How long the player will get damaged")]
    [SerializeField] private float damageInterval = 3f;

    [Tooltip("instant death, literraly like it say")]
    [SerializeField] private bool isInstantDeath = false;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (collision.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                if (isInstantDeath == true)
                {
                    playerHealth.TakeDamage(999999); // Insane amount just incase if the robot is gojo satoru
                }
                if (Time.time >= nextDamageTime && isInstantDeath == false)
                {
                    playerHealth.TakeDamage(areaDamage);
                    nextDamageTime = Time.time + damageInterval;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nextDamageTime = 0f; // reset timer when player exit
        }
    }
}
