using UnityEngine;
using _Game.Scripts.Cogs;

namespace _Game.Scripts.Cogs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CogsDrag : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private float groundCheckDistance = 0.3f;
        [SerializeField] private LayerMask checkLayers;

        [Header("Drag & Throw Settings")]
        [SerializeField] private float dragSpeed = 25f;
        [SerializeField] private float throwForceMultiplier = 1.2f;
        [SerializeField] private float dragResistance = 10f;

        [Header("Telekinetic Range")]
        [SerializeField] private float maxDragDistance = 7f;
        private Transform _playerTransform; 

        public bool onDrag { get; private set; }

        private Vector3 _offset;
        private float _zDepth;
        private bool _isColliding;
        private Rigidbody2D _rb;
        private Cogs _cogs;
        private Vector2 _dragVelocity;
        private float _originalLinearDrag;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _cogs = GetComponent<Cogs>();
            _originalLinearDrag = _rb.drag;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _playerTransform = player.transform;
        }

        void Update()
        {
            HandleDragLogic();
            HandleReleaseLogic();
        }

        private void HandleDragLogic()
        {
            if (onDrag)
            {
                Vector3 targetPos = MouseWorldPos() + _offset;

                if (_playerTransform != null)
                {
                    float dist = Vector3.Distance(_playerTransform.position, transform.position);
                    
                    if (dist > maxDragDistance)
                    {
                        ForceRelease();
                        return;
                    }
                }

                Vector2 direction = (targetPos - transform.position);
                _rb.velocity = direction * dragSpeed;
                _dragVelocity = _rb.velocity;
            }
        }

        private void OnMouseDown()
        {
            if (!gameObject.CompareTag("Cogs")) return;

            if (_playerTransform != null)
            {
                float dist = Vector3.Distance(_playerTransform.position, transform.position);
                if (dist > maxDragDistance) return; 
            }

            _zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
            _offset = transform.position - MouseWorldPos();
            onDrag = true;

            _rb.isKinematic = false;
            _rb.gravityScale = 0f;
            _rb.drag = dragResistance;
        }

        public void ForceRelease()
        {
            if (!onDrag) return;
            
            onDrag = false;
            _rb.drag = _originalLinearDrag;
            _rb.isKinematic = false;
            _rb.gravityScale = 1f;
            
            _rb.velocity = Vector2.down * 2f;
        }

        private void OnDrawGizmosSelected()
        {
            if (_playerTransform != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_playerTransform.position, maxDragDistance);
                
                if (onDrag)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(_playerTransform.position, transform.position);
                }
            }
        }

        private void OnMouseUp()
        {
            if (!onDrag) return;
            onDrag = false;
            _rb.drag = _originalLinearDrag;
            if (_cogs != null && !_cogs.isSnapped)
            {
                _rb.gravityScale = 1f;
                _rb.velocity = _dragVelocity * throwForceMultiplier;
            }
        }

        private void HandleReleaseLogic()
        {
            if (onDrag || (_cogs != null && _cogs.isSnapped)) return;
            if (_rb.velocity.magnitude > 0.2f)
            {
                _rb.isKinematic = false;
                _rb.gravityScale = 1f;
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
}