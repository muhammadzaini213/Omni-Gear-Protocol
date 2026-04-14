using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogsDrag : MonoBehaviour
{
    private bool onDrag = false;
    private Vector3 offset;
    private float zDepth;

    void Start() { }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
           GameObject clickedObject = hit.collider.gameObject;
            
           if (clickedObject.tag == "Cogs")
           {
                onDrag = !onDrag;
           }
           else
           {
               onDrag = !onDrag;
           }
        }
        Hold();
    }

    private void Hold()
    {
        zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        if (onDrag)
        {
            this.transform.position = GetMouseWorldPos();
        }
    }

    private Vector3 GetMouseWorldPos()
    {

        
        Vector3 mouseScreenPos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            zDepth
        );
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}