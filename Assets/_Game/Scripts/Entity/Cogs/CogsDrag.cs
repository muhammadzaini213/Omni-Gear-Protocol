using UnityEngine;

public class CogsDrag : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask checkLayers;

    public bool onDrag { get; private set; }
    private Vector3 _offset;
    private float   _zDepth;
    private bool    _isColliding;
    private Rigidbody2D _rb;

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    void Update() { Hold(); Release(); }

    private void OnMouseDown()
    {
        if (!gameObject.CompareTag("Cogs")) return;

        _zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - MouseWorldPos();
        onDrag  = true;
        _rb.isKinematic = true;
    }

    private void OnMouseUp() => onDrag = false;

    private void Hold()
    {
        if (onDrag) transform.position = MouseWorldPos() + _offset;
    }

    private void Release()
    {
        if (onDrag) return;

        var hit = Physics2D.Raycast(transform.position, Vector2.down,
                                    groundCheckDistance, checkLayers);
        bool shouldFall = hit.collider == null && !_isColliding;

        if (shouldFall)
        {
            _rb.isKinematic = false;
            _rb.gravityScale = 0.2f;
        }
        else
        {
            _rb.isKinematic  = true;
            _rb.gravityScale = 0f;
            _rb.velocity     = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D _) => _isColliding = true;
    private void OnCollisionExit2D(Collision2D _)  => _isColliding = false;

    private Vector3 MouseWorldPos()
    {
        var p = Input.mousePosition;
        p.z = _zDepth;
        return Camera.main.ScreenToWorldPoint(p);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
    }
#endif
}