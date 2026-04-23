using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PreMenuScreen : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup cg;
    [SerializeField] private float fadeSpeed = 1.5f;
    private bool isFading = false;

    private const string INTRO_KEY = "HasSeenIntro";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(INTRO_KEY, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        cg = GetComponent<CanvasGroup>();
        
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        DontDestroyOnLoad(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFading)
        {
            PlayerPrefs.SetInt(INTRO_KEY, 1);
            PlayerPrefs.Save(); 
            
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        isFading = true;

        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        cg.blocksRaycasts = false;
        cg.interactable = false;
        
        Destroy(gameObject);
    }
}