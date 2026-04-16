using System;
using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Snapper : MonoBehaviour
    {
        private bool isCogSnapped;
        [SerializeField] public SnappedEvent _nappedEvent;

        private Color objColor; // this is for testing purpose(t.p) only

        void Start()
        {
              objColor = GetComponent<SpriteRenderer>().color; // t.p  
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Cogs"))
            {
                var drag = other.GetComponent<CogsDrag>();

                if (drag != null && !Input.GetMouseButton(0)) 
                {
                    SnapCogsToTransform(other.gameObject);
                    isCogSnapped =  true;
                    SnappedEvent.Subscribe(OnCogsSnappedAction);
                    SnappedEvent.OnCogSnappedEvent();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Cogs"))
            {
                other.transform.parent = null;
                isCogSnapped =  false;
                SnappedEvent.Unsubscribe(OnCogsSnappedAction);
                SnappedEvent.Subscribe(OnCogsUnsnappedAction);
                SnappedEvent.OnCogSnappedEvent();
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

        
        //Defines Cogs Event Action 
        private void OnCogsSnappedAction()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = new Color(0f, 0f, 0f, 1f); 
        }
        
        private void OnCogsUnsnappedAction()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = objColor;
        }
        
    }
}