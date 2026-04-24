using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string aboutMenuSceneName = "AboutMenu";
    [SerializeField] private string settingsMenuSceneName = "SettingsMenu";
    [SerializeField] private AudioClip clickSound;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
        PlayerPrefs.SetInt("CheckpointIndex", -1); // reset checkpoint 
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }


    public void About()
    {
        SceneManager.LoadScene(aboutMenuSceneName);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }

    public void Settings()
    {
        SceneManager.LoadScene(settingsMenuSceneName);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }

    public void Exit()
    {
        SfxPlayer.Instance.PlayUISfx(clickSound);
        Application.Quit();
    }
}