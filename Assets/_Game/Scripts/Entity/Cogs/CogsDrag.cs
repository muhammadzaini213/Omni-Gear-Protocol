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
        [SerializeField] private float maxDragDistance = 7f; // Jarak maksimum dari Player
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

            // Mencari player berdasarkan Tag "Player"
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

                // --- LOGIKA JARAK MAKSIMUM ---
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

            // Cek jarak sebelum mengizinkan drag pertama kali
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
            
            // Berikan sedikit gaya jatuh agar tidak melayang diam
            _rb.velocity = Vector2.down * 2f;
        }

        // --- GIZMOS UNTUK VISUALISASI DI EDITOR ---
        private void OnDrawGizmosSelected()
        {
            // Jika ingin melihat radius jangkauan dari Player saat objek dipilih
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

        // ... Sisa fungsi OnMouseUp, HandleReleaseLogic, dll tetap sama ...
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