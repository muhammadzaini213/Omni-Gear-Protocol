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
            if (_isCogSnapped) return;
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
            cogObj.transform.position = transform.position;
            cogObj.transform.SetParent(transform);
            cog?.Snap(); // NEW SCRIPT: Notify the cog that it has been snapped, so it can start rotating

            var rb = cogObj.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.isKinematic = true; rb.velocity = Vector2.zero; }

            _currentCog = cog;
            _isCogSnapped = true;



            snapChannel.RaiseSnapped(cogObj, cog.cogType);
        }

        private void UnsnapCog()
        {
            var cogObj = _currentCog.gameObject;
            var type = _currentCog.cogType;

            Cogs cogs = cogObj.GetComponent<Cogs>();
            cogs?.UnsnapNotify(); // NEW SCRIPT: Notify the cog that it has been unsnapped, so it can stop rotating

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