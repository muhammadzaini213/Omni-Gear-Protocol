using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private string MainMenuScene = "MainMenu";

    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip clickSound;
    void Start()
    {
        MusicPlayer.Instance.PlayOneShot(winSfx);
        SfxPlayer.Instance.StopAllCoroutines();
        SfxPlayer.Instance.StopAllLoopingSfx();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
        SfxPlayer.Instance.PlayUISfx(clickSound);

    }
}