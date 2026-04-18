using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Unused for now
    // private PlayerEnergy playerEnergy;

    // ==================== MOVEMENT ====================
    // Using Horizontal Input Instead of Vector2 because we only need to move left and right, for simplified ofc
    private float horizontalInput;
    private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float jumpPower = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float fallmultiplier = 2f;
    [SerializeField] private float jumpCutMultiplier = 0.8f;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        FlipCharacter();

        animator.SetBool("isRunning", horizontalInput != 0);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new UnityEngine.Vector2(horizontalInput * normalSpeed, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void FlipCharacter()
    {
        if (horizontalInput == 0) return;

        UnityEngine.Vector3 currentScale = transform.localScale;

        float direction = Mathf.Sign(horizontalInput);

        transform.localScale = new UnityEngine.Vector3(
            direction * Mathf.Abs(currentScale.x),
            currentScale.y,
            currentScale.z
        );
    }

    private void Jump()
    {
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpPower);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}