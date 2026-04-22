using _Game.Scripts.Cogs;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Elevator : BaseTriggerObj
{
    [Header("Elevator Movement Settings")]
    [Tooltip("The position where the elevator starts (unpowered)")]
    [SerializeField] private Transform startPoint;

    [Tooltip("The position the elevator moves to when a cog is attached")]
    [SerializeField] private Transform endPoint;

    [Tooltip("How fast the elevator moves")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AudioClip elevatorClip;

    private Rigidbody2D rb;
    private bool isPowered = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Elevator] {cog.name} ({type}) attached to {gameObject.name}");
        isPowered = true;
        SfxPlayer.Instance.PlayEnvironmentSfx(elevatorClip);
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        Debug.Log($"[Elevator] {cog.name} ({type}) detached from {gameObject.name}");
        isPowered = false;
        SfxPlayer.Instance.PlayEnvironmentSfx(elevatorClip);
    }

    private void FixedUpdate()
    {
        if (startPoint == null || endPoint == null) return;
        Vector2 targetPosition = isPowered ? endPoint.position : startPoint.position;
        Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}