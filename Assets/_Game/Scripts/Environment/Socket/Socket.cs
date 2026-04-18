using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Socket : MonoBehaviour
    {
        [Header("Snap Settings")]
        [SerializeField] private CogsType[] allowedTypes;
        [SerializeField] private CogSnapChannel snapChannel;
        private bool _isCogSnapped;
        private Cogs _currentCog;
        private CogsDrag _currentDrag;

        private void Update()
        {
            if (!_isCogSnapped || _currentCog == null) return;

            if (!_currentDrag.onDrag)
            {
                _currentCog.transform.position = transform.position;
            }
            else
            {
                UnsnapCog();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isCogSnapped) return;

            if (!IsAllowedCog(other, out Cogs cog)) return;

            var drag = other.GetComponent<CogsDrag>();
            if (drag == null || drag.onDrag || Input.GetMouseButton(0)) return;

            SnapCog(other.gameObject, cog);
        }

        private void SnapCog(GameObject cogObj, Cogs cog)
        {
            _isCogSnapped = true;
            _currentCog = cog;
            _currentDrag = cogObj.GetComponent<CogsDrag>();

            cogObj.transform.SetParent(transform);
            cogObj.transform.localPosition = Vector3.zero;
            
            var rb = cogObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.simulated = true;
            }

            cog?.Snap();
            snapChannel.RaiseSnapped(cogObj, cog.cogType);
        }

        public void UnsnapCog()
        {
            if (_currentCog == null) return;

            var cogObj = _currentCog.gameObject;
            var type = _currentCog.cogType;

            var rb = cogObj.GetComponent<Rigidbody2D>();
            if (rb != null) rb.isKinematic = false;

            _currentCog.UnsnapNotify();
            cogObj.transform.SetParent(null);
            
            _currentDrag = null;
            _currentCog = null;
            _isCogSnapped = false;

            snapChannel.RaiseUnsnapped(cogObj, type);
        }

        private bool IsAllowedCog(Collider2D other, out Cogs cog)
        {
            cog = other.GetComponent<Cogs>();
            if (cog == null) return false;

            foreach (var t in allowedTypes)
                if (t == cog.cogType) return true;

            return false;
        }
    }
}