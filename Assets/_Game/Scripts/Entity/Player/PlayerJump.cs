using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerJump : BaseTriggerObj, ISocketAttached
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    [SerializeField] private float jumpPower = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float fallmultiplier = 2f;
    [SerializeField] private float jumpCutMultiplier = 0.8f;

    private bool canJump;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (!canJump)
        {
            Debug.Log("Player cannot jump - no big cog attached");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.Play("Player_Fall");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpPower);
        isGrounded = false;
    }

    public override void OnCogAttached(GameObject cog, CogsType type)
    {
        canJump = true;
    }

    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        canJump = false;
    }
}