using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerJump : BaseTriggerObj, ISocketAttached
{
    private Rigidbody2D rb;
    private Animator animator;
    
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded { get; private set; }

    [Header("Jump Config")]
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float fallMultiplier = 2f;
    [SerializeField] private float jumpCutMultiplier = 0.8f;

    private bool canJump;
    private bool wasGrounded;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        CheckGroundStatus();

        // Update Animator
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (!canJump || animator.GetBool("isDeath")) return;

        // Input Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Variable Jump Height (Jump Cut)
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    private void CheckGroundStatus()
    {
        wasGrounded = isGrounded;
        
        // Cek apakah lingkaran di kaki menyentuh layer Ground
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // Landing Logic: Jika frame sebelumnya melayang dan sekarang menyentuh tanah
        if (isGrounded && !wasGrounded)
        {
            if (!animator.GetBool("isDeath"))
            {
                animator.Play("Player_Fall");
            }
        }
    }

    void FixedUpdate()
    {
        // Custom Gravity untuk Fall Feel yang lebih berat
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        // Kita tidak perlu set isGrounded = false manual karena CheckGroundStatus akan melakukannya
    }

    public override void OnCogAttached(GameObject cog, CogsType type) => canJump = true;
    public override void OnCogDetached(GameObject cog, CogsType type) => canJump = false;

    // Untuk visualisasi radius di Editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}