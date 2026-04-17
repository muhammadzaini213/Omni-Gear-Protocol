using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;
        public float rotateSpeed;
        public bool reverseDirection;
        public bool isSnapped;
        public CogSnapChannel snapChannel;
        

        void Update() => RotateCogs();

        public void RotateCogs()
        {
            if(!isSnapped) return;
            
            float direction = reverseDirection == true ? -1f: 1f;
            transform.Rotate(0f, 0f, direction * rotateSpeed * Time.deltaTime);
        }

        public void SnapNotify(GameObject obj, CogsType type)
        {
            isSnapped = true;
        }

        public void UnsnapNotify(GameObject obj, CogsType type)
        {
            isSnapped = false;
        }
        
     
        private void OnEnable()
        {
            snapChannel.OnCogSnapped += SnapNotify;
            snapChannel.OnCogUnsnapped += UnsnapNotify;
        }

        private void OnDisable()
        {
            snapChannel.OnCogSnapped -= SnapNotify;
            snapChannel.OnCogUnsnapped -= UnsnapNotify;
        }
    }

    public enum CogsType { Small, Medium, Large }
}