using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;
        public float rotateSpeed;
        public bool reverseDirection;
        public bool isSnapped { get; private set; }

        void Update() => RotateCogs();

        public void RotateCogs()
        {
            if (!isSnapped) return;

            float direction = reverseDirection ? -1f : 1f;
            transform.Rotate(0f, 0f, direction * rotateSpeed * Time.deltaTime);
        }

        public void Snap() => isSnapped = true;

        public void UnsnapNotify() => isSnapped = false;
    }

    public enum CogsType { Small, Medium, Large }
}