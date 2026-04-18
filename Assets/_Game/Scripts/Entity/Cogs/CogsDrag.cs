using UnityEngine;
using _Game.Scripts.Cogs;

public class CogsDrag : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask checkLayers;
    [SerializeField] private float throwForceMultiplier = 1.2f;

    public bool onDrag { get; private set; }
    private Vector3 _offset;
    private float _zDepth;
    private bool _isColliding;
    private Rigidbody2D _rb;
    private Cogs _cogs;

    private Vector3 _lastPosition;
    private Vector2 _dragVelocity;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cogs = GetComponent<Cogs>();
    }

    void Update()
    {
        Hold();
        Release();
    }

    private void OnMouseDown()
    {
        if (!gameObject.CompareTag("Cogs")) return;

        _zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - MouseWorldPos();
        onDrag = true;

        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0f;

        _lastPosition = transform.position; 
    }

    private void OnMouseUp()
    {
        if (!onDrag) return;
        onDrag = false;

        if (_cogs != null && !_cogs.isSnapped)
        {
            _rb.isKinematic = false;
            _rb.gravityScale = 0.5f;

            _rb.velocity = _dragVelocity * throwForceMultiplier;
        }
    }

    private void Hold()
    {
        if (onDrag)
        {
            Vector3 currentPos = MouseWorldPos() + _offset;

            _dragVelocity = (currentPos - _lastPosition) / Time.deltaTime;
            _lastPosition = currentPos;

            transform.position = currentPos;
        }
    }

    private void OnDisable()
    {
        _isColliding = false;
        onDrag = false;
    }

    private void Release()
    {
        if (onDrag || (_cogs != null && _cogs.isSnapped)) return;

        if (_rb.velocity.magnitude > 0.2f)
        {
            _rb.isKinematic = false;
            _rb.gravityScale = 1f; // Pastikan gravitasi normal
            return;
        }

        var hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, checkLayers);

        if (hit.collider != null || _isColliding)
        {
            _rb.isKinematic = true;
            _rb.gravityScale = 0f;
            _rb.velocity = Vector2.zero;
        }
        else
        {
            _rb.isKinematic = false;
            _rb.gravityScale = 1f;
        }
    }
    private void OnCollisionEnter2D(Collision2D _) => _isColliding = true;
    private void OnCollisionExit2D(Collision2D _) => _isColliding = false;

    private Vector3 MouseWorldPos()
    {
        var p = Input.mousePosition;
        p.z = _zDepth;
        return Camera.main.ScreenToWorldPoint(p);
    }
}