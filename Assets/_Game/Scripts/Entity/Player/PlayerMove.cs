using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;

    // Unused for now
    // private PlayerEnergy playerEnergy;

    // ==================== MOVEMENT ====================
    // Using Horizontal Input Instead of Vector2 because we only need to move left and right, for simplified ofc
    private float horizontalInput;
    private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 10f;
    [SerializeField] private float jumpPower = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float fallmultiplier = 2.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

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