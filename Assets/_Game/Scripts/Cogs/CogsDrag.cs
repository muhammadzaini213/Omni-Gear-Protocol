using UnityEngine;
using UnityEngine.Serialization;

public class CogsDrag : MonoBehaviour
{
    [SerializeField] private bool onDrag = false;
    private Vector3 offset;
    private float zDepth;
    [SerializeField] float groundCheckDistance = 0.2f;
    [FormerlySerializedAs("groundLayer")] [SerializeField] LayerMask checkLayers;
    
    [SerializeField] public bool shouldFall;
    
    private Rigidbody2D rb;
    
    [SerializeField] bool isColliding = false;

    // Change these to 2D
    void OnCollisionEnter2D(Collision2D col) => isColliding = true;
    void OnCollisionExit2D(Collision2D col) => isColliding = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Hold();
        Release();
    }

    private void OnMouseDown()
    { 
        if (gameObject.CompareTag("Cogs"))
        {
            zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPos();
            onDrag = true;
            shouldFall = false;
            rb.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        onDrag = false;
        shouldFall = true;
    }

    private void Hold()
    {
        if (onDrag)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private void Release()
    {
            if(onDrag) return;
            
            RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, checkLayers);
            shouldFall = (groundCheck.collider == null);
            
    
            if (shouldFall && !isColliding)
            {
                rb.isKinematic = false;
                rb.gravityScale = 0.2f;
            }
            else
            {
                rb.isKinematic = true;
                rb.gravityScale = 0.0f;
                rb.velocity = Vector2.zero; 
            }
    }
    
    

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zDepth; 
        
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
    private void OnDrawGizmos()
    {
        // Only draw if we are in Play Mode and have a transform
        Gizmos.color = Color.red;
    
        // Draw the ground check ray
        // We draw it starting from the object's position, pointing down
        Vector3 direction = Vector3.down * 0.07f;
        Gizmos.DrawRay(transform.position, direction);

        // If you want to see the 'offset' point while dragging
        if (onDrag)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(GetMouseWorldPos() + offset, groundCheckDistance);
        }
    }
}