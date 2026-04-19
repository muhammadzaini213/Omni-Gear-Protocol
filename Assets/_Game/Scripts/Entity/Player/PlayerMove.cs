using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerMove : BaseTriggerObj, ISocketAttached
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalInput;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 3f;
    private bool canMove;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("isRunning", horizontalInput != 0);
    }

    void FixedUpdate()
    {
        if (!canMove) {
            Debug.Log("Player cannot move - no small cog attached");
            return;
        }
        rb.velocity = new UnityEngine.Vector2(horizontalInput * normalSpeed, rb.velocity.y);

    }

    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        canMove = true;
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        canMove = false;
    }
}