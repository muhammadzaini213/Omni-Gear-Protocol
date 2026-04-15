using System;
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
        Hold();
    }

    private void OnMouseDown()
    {
        
        if (gameObject.CompareTag("Cogs"))
        {
            Debug.Log("OnMouseDown");
            zDepth = Camera.main.WorldToScreenPoint(transform.position).z;
            offset = transform.position - GetMouseWorldPos();
            onDrag = true;
        }
    }

    private void OnMouseUp()
    {
        onDrag = false;
    }

    private void Hold()
    {
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