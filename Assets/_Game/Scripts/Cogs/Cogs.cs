using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;
        public float rotateSpeed;

        void Update()
        {
            RotateCogs();
        }
        public void RotateCogs()
        {
            this.transform.Rotate(0f, 0f, this.rotateSpeed * Time.deltaTime);
        }
    }

    public enum CogsType
    {
        Small,
        Medium,
        Large   
    }
}