using _Game.Scripts.Cogs;
using UnityEngine;

namespace _Game.Scripts
{
    public class VFXAnimHandler: BaseTriggerObj
    {
        Animator animator;
        PlayerMove playerMove;
        bool isGrounded;
        private bool isCogMissing;
        float speed;

        public Transform movementVFX;
        
        void Start()
        {
            animator = GameObject.Find("VFX_Player").GetComponent<Animator>();
            playerMove = GetComponent<PlayerMove>();
            
        }

        void Update()
        {
            isGrounded = playerMove.IsGrounded;
            speed = playerMove.speed;

            if (speed > 0.1f || speed < -0.1f)
            {
                animator.gameObject.transform.position = movementVFX.position;
            }
            
            animator.SetBool("Spark", isCogMissing);
            animator.SetBool("Ground", isGrounded);
            animator.SetFloat("Speed", speed);
        }
        public override void OnCogAttached(GameObject cog, CogsType type)
        {
            isCogMissing = false;
            animator.SetTrigger("Smoke");
            animator.gameObject.transform.position = cog.transform.position;
            animator.gameObject.transform.rotation = cog.transform.rotation;
        }

        public override void OnCogDetached(GameObject cog, CogsType type)
        {
            isCogMissing = true;
            animator.gameObject.transform.position = movementVFX.position;
        }
    }
}