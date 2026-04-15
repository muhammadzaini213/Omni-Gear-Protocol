using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Snapper : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Cogs"))
            {
                var drag = other.GetComponent<CogsDrag>();

                if (drag != null && !Input.GetMouseButton(0)) 
                {
                    SnapCogsToTransform(other.gameObject);
                }
            }
        }

        private void SnapCogsToTransform(GameObject collidedObj)
        {
            collidedObj.transform.position = transform.position;
            collidedObj.transform.parent = this.transform;
            
            var rb = collidedObj.GetComponent<Rigidbody2D>();
            if(rb != null) {
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
            }
        }
    }
}