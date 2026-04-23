using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private string GameScene = "GameScene";
    [SerializeField] private string MainMenuScene = "MainMenu";

    [SerializeField] private AudioClip gameOverSfx;
    [SerializeField] private AudioClip clickSound;
    void Start()
    {
        MusicPlayer.Instance.PlayOneShot(gameOverSfx);
        SfxPlayer.Instance.StopAllCoroutines();
        SfxPlayer.Instance.StopAllLoopingSfx();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(GameScene);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
        SfxPlayer.Instance.PlayUISfx(clickSound);
    }
}