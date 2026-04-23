using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreMenuScreen : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField] private string menuScene = "MainMenu";
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
        }

        if (cg.alpha <= 0.5)
        {
            fadeOutUI = false;
            SceneManager.LoadScene(menuScene);
        }
    }

    public void OnMouseOver()
    {
       fadeOutUI = true;
    }
}
