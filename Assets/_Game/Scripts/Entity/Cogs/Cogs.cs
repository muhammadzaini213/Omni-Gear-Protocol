using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;
        public float rotateSpeed;
        public bool reverseRotation;

        void Update() => RotateCogs();

        public void RotateCogs()
        {
            float direction = reverseRotation == true ? -1 : 1;
            transform.Rotate(0f, 0f, direction * rotateSpeed * Time.deltaTime);
        }
    }

    public enum CogsType { Small, Medium, Large }
}