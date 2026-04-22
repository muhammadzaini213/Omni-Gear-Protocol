using _Game.Scripts.Cogs;
using UnityEngine;

public class PlayerMove : BaseTriggerObj, ISocketAttached
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalInput;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 3f;
    private PlayerJump playerJump;
    private bool canMove;

    [Header("VFX Settings")]
    public Animator vfxAnimator;
    public Transform movementVFXPos;
    public Transform sparkVFXPos;
    [SerializeField] private string runVFXStateName = "Dust";
    [SerializeField] private string sparkVFXStateName = "Spark";
    [SerializeField] private string idleVFXStateName = "Idle";

    [Header("Audio Settings")]
    [SerializeField] private AudioClip runSFX;
    [SerializeField] private AudioClip sparkSFX;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        float rawInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = canMove ? rawInput : 0f;

        HandleVFXAndSFX(rawInput);
        
        animator.SetBool("isRunning", horizontalInput != 0);
    }

    private void HandleVFXAndSFX(float rawInput)
    {
        if (vfxAnimator == null) return;

        Vector3 targetPos;

        // KONDISI 1: Mencoba bergerak tapi tidak punya Cog (SPARK)
        if (!canMove && rawInput != 0)
        {
            targetPos = sparkVFXPos.position;
            HandleVFXScale(rawInput);
            vfxAnimator.Play(sparkVFXStateName);

            // Audio Logic
            SfxPlayer.Instance.PlayPlayerSfx(sparkSFX, 1f, true); // Play Spark Loop
            SfxPlayer.Instance.StopLoopingSfx(runSFX);            // Stop Run
        }
        // KONDISI 2: Bergerak normal dengan Cog (RUN)
        else if (horizontalInput != 0)
        {
            targetPos = CalculateVFXPosition(horizontalInput);
            HandleVFXScale(horizontalInput);

            if (playerJump.isGrounded)
            {
                vfxAnimator.Play(runVFXStateName);
                SfxPlayer.Instance.PlayPlayerSfx(runSFX, 1f, true); // Play Run Loop
            }
            else
            {
                vfxAnimator.Play(idleVFXStateName);
                SfxPlayer.Instance.StopLoopingSfx(runSFX); // Berhenti bunyi di udara
            }

            SfxPlayer.Instance.StopLoopingSfx(sparkSFX); // Stop Spark
        }
        // KONDISI 3: Diam (IDLE)
        else
        {
            targetPos = movementVFXPos.position;
            vfxAnimator.Play(idleVFXStateName);

            // Stop All Movement Sounds
            SfxPlayer.Instance.StopLoopingSfx(runSFX);
            SfxPlayer.Instance.StopLoopingSfx(sparkSFX);
        }

        vfxAnimator.gameObject.transform.position = targetPos;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (vfxAnimator != null) vfxAnimator.Play(idleVFXStateName);
        
        // Pastikan suara mati saat script disable/player mati
        if (SfxPlayer.Instance != null)
        {
            SfxPlayer.Instance.StopLoopingSfx(runSFX);
            SfxPlayer.Instance.StopLoopingSfx(sparkSFX);
        }
    }

    private void HandleVFXScale(float input)
    {
        Vector3 vfxScale = vfxAnimator.gameObject.transform.localScale;
        vfxScale.x = (input > 0) ? Mathf.Abs(vfxScale.x) : -Mathf.Abs(vfxScale.x);
        vfxAnimator.gameObject.transform.localScale = vfxScale;
    }

    private Vector3 CalculateVFXPosition(float input)
    {
        if (input < 0)
        {
            Vector3 localOffset = movementVFXPos.localPosition;
            Vector3 mirroredLocalPos = new Vector3(-localOffset.x, localOffset.y, localOffset.z);
            return movementVFXPos.parent.TransformPoint(mirroredLocalPos);
        }
        return movementVFXPos.position;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        rb.velocity = new Vector2(horizontalInput * normalSpeed, rb.velocity.y);
    }

    public override void OnCogAttached(GameObject cog, CogsType type) => canMove = true;
    
    public override void OnCogDetached(GameObject cog, CogsType type)
    {
        canMove = false;
        // Langsung stop suara jalan jika Cog dicopot paksa
        SfxPlayer.Instance.StopLoopingSfx(runSFX);
    }
}