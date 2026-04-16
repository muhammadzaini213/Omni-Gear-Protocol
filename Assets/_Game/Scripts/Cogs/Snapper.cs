using System;
using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Snapper : MonoBehaviour
    {
        private bool isCogSnapped;
        private GameObject attachedObject;
        private Color objColor; // this is for testing purpose(t.p) only

        void Start()
        {
            objColor = GetComponent<SpriteRenderer>().color; // t.p  
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            if(other.gameObject.tag == "Environment") return;
            SnappedEvent.Subscribe(OnCogsSnappedAction);
            SnappedEvent.OnCogSnappedEvent();
            CogsEvent.BroadcastCogAttached(this.gameObject, GetComponent<Cogs>().cogType );
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.tag == "Environment") return;
            var drag = GetComponent<CogsDrag>();
        
            if (drag != null && !Input.GetMouseButton(0) && !drag.onDrag) 
            {
               SnapCogsToTransform(other.gameObject);
               isCogSnapped =  true;
            }
        }
        

        private void OnTriggerExit2D(Collider2D other)
        {
                Debug.Log(other.gameObject.name);
                if(other.gameObject.tag == "Environment") return;
                transform.parent = null;
                isCogSnapped =  false;
                SnappedEvent.Unsubscribe(OnCogsSnappedAction);
                SnappedEvent.Subscribe(OnCogsUnsnappedAction);
                SnappedEvent.OnCogSnappedEvent();
        }

        private void SnapCogsToTransform(GameObject collidedObj)
        {
            transform.position = collidedObj.transform.position;
            this.transform.parent = collidedObj.transform ;
            
            var rb = GetComponent<Rigidbody2D>();
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
            Debug.Log("Cogs Event triggered by" + this.gameObject.name);
        }
        
        private void OnCogsUnsnappedAction()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = objColor;
        }
        
    }
}