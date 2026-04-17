using UnityEngine;

public class PlayerHealth : Health
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        playerMove = GetComponentInParent<PlayerMove>();
    }
    protected override void Start()
    {
        base.Start();
        OnDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died! Show Game Over.");
        animator.SetTrigger("Death");

        if (playerMove != null)
        {
            playerMove.enabled = false;
        }
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public override void TakeDamage(int amount)
    {
        int reduced = Mathf.Max(amount - 5, 0); // contoh: armor 5
        base.TakeDamage(reduced);
    }
}