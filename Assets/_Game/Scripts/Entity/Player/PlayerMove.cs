using System;
using System.Numerics;
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
    }

    void FixedUpdate()
    {
        rb.velocity = new UnityEngine.Vector2(horizontalInput * normalSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpPower);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}