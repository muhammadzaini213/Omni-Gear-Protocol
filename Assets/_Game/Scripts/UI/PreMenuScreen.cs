using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMenuScreen : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField]private CanvasGroup menuCg;
    private bool fadeOutUI;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (fadeOutUI)
        {
            cg.alpha -= Time.deltaTime;
            menuCg.alpha += Time.deltaTime;
        }

        if (cg.alpha <= 0)
        {
            fadeOutUI = false;
            gameObject.SetActive(false);
        }
    }

    public void OnMouseOver()
    {
       fadeOutUI = true;
    }
}
