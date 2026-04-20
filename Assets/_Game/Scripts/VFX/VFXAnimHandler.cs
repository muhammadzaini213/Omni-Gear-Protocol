using _Game.Scripts.Cogs;
using UnityEngine;

namespace _Game.Scripts
{
    public class VFXAnimHandler : BaseTriggerObj
    {
        private Animator animator;
        private PlayerJump playerJump;
        private bool isCogMissing;

        public Transform movementVFX;
        
        void Start()
        {
            animator = GameObject.Find("VFX_Player").GetComponent<Animator>();
            playerJump = GetComponent<PlayerJump>();
        }

        void Update()
        {
            // Biarkan handler ini mengurus status "Grounded" dan "Spark" saja
            animator.SetBool("Ground", playerJump.isGrounded);
            animator.SetBool("Spark", isCogMissing);
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