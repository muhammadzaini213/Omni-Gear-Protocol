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

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        if (vfxAnimator == null) return;

        float rawInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = canMove ? rawInput : 0f;

        animator.SetBool("isRunning", horizontalInput != 0);

        Vector3 targetPos;

        if (!canMove && rawInput != 0)
        {
            targetPos = sparkVFXPos.position;
            HandleVFXScale(rawInput);
            vfxAnimator.Play(sparkVFXStateName);
        }
        else if (horizontalInput != 0)
        {
            targetPos = CalculateVFXPosition(horizontalInput);
            HandleVFXScale(horizontalInput);

            if (playerJump.isGrounded)
                vfxAnimator.Play(runVFXStateName);
            else
                vfxAnimator.Play(idleVFXStateName);
        }
        else
        {
            targetPos = movementVFXPos.position;
            vfxAnimator.Play(idleVFXStateName);
        }

        vfxAnimator.gameObject.transform.position = targetPos;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        vfxAnimator.Play(idleVFXStateName);
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
    public override void OnCogDetached(GameObject cog, CogsType type) => canMove = false;
}