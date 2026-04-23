using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AudioClip clickSound;
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }
}