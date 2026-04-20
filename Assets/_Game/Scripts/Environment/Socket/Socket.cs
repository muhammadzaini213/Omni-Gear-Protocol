using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Socket : MonoBehaviour
    {
        [Header("Snap Settings")]
        [SerializeField] protected CogsType[] allowedTypes;
        [SerializeField] private CogSnapChannel snapChannel;

        [Header("VFX Settings")]
        [SerializeField] private Animator vfxAnimator;
        [SerializeField] private string smokeAnimationName = "Smoke";

        private bool _isCogSnapped;
        private Cogs _currentCog;
        private CogsDrag _currentDrag;

        protected virtual void Update()
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

            if (cog.isSnapped) return;


            var drag = cog.GetComponent<CogsDrag>();

            if (drag == null || drag.onDrag) return;

            SnapCog(cog.gameObject, cog);
        }

        protected virtual void SnapCog(GameObject cogObj, Cogs cog)
        {
            _isCogSnapped = true;
            _currentCog = cog;
            _currentDrag = cogObj.GetComponent<CogsDrag>();

            cogObj.transform.SetParent(transform);
            cogObj.transform.localPosition = Vector3.zero;

            if (vfxAnimator != null)
            {
                vfxAnimator.gameObject.transform.position = transform.position;
                vfxAnimator.Play(smokeAnimationName);
            }

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