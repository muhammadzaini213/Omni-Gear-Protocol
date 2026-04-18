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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isCogSnapped)
            {
                if (_currentCog != null && other.gameObject == _currentCog.gameObject)
                {
                    var currentDrag = other.GetComponent<CogsDrag>();
                    if (currentDrag != null && currentDrag.onDrag)
                    {
                        UnsnapCog();
                    }
                }
                return;
            }

            if (!IsAllowedCog(other, out Cogs cog)) return;

            var drag = other.GetComponent<CogsDrag>();
            if (drag == null || drag.onDrag || Input.GetMouseButton(0)) return;

            SnapCog(other.gameObject, cog);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_currentCog == null) return;
            if (other.gameObject != _currentCog.gameObject) return;

            UnsnapCog();
        }

        private void SnapCog(GameObject cogObj, Cogs cog)
        {
            _isCogSnapped = true;
            _currentCog = cog;

            cogObj.transform.position = transform.position;
            cogObj.transform.SetParent(transform);
            
            var rb = cogObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                rb.simulated = false;
            }

            cog?.Snap();
            snapChannel.RaiseSnapped(cogObj, cog.cogType);
        }

        private void UnsnapCog()
        {
            var cogObj = _currentCog.gameObject;
            var type = _currentCog.cogType;

            var rb = cogObj.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = true;

            _currentCog.UnsnapNotify();
            cogObj.transform.SetParent(null);
            
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