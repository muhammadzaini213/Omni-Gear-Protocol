using UnityEngine;
using UnityEngine.SceneManagement;
using _Game.Scripts.Cogs;

public class PlayerHealth : Health
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private PlayerTelekinetic playerTelekinetic;
    private PlayerSocket[] playerSockets;
    [SerializeField] private string GameOverScene = "GameOver";
    [SerializeField] private AudioClip deathSound;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();

        playerMove = GetComponentInChildren<PlayerMove>();
        playerJump = GetComponentInChildren<PlayerJump>();
        playerTelekinetic = GetComponentInChildren<PlayerTelekinetic>();
        playerSockets = GetComponentsInChildren<PlayerSocket>();
    }

    protected override void Start()
    {
        base.Start();
        OnDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        SfxPlayer.Instance.PlayPlayerSfx(deathSound);
        if (playerMove != null) playerMove.enabled = false;
        if (playerJump != null) playerJump.enabled = false;
        if (playerTelekinetic != null) playerTelekinetic.enabled = false;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isGrounded", false);
            animator.SetFloat("yVelocity", 0);
            animator.SetBool("isDeath", true);
        }


        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            rb.isKinematic = false;

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        foreach (var socket in playerSockets)
        {
            socket.EjectAllCogsOnDeath();
        }

        Invoke(nameof(GameOver), 2);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(GameOverScene);
    }
}