using UnityEngine;
using System.Collections;

namespace _Game.Scripts.Cogs
{
    public class PlayerSocket : Socket
    {
        [Header("Recall Config")]
        [SerializeField] private KeyCode recallKey = KeyCode.R;
        [SerializeField] private float flySpeed = 15f;

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(recallKey))
            {
                TryRecall();
            }
        }

        private void TryRecall()
        {
            Cogs targetCog = FindTargetCog();

            if (targetCog != null)
            {
                if (targetCog.transform.parent == transform) return;

                StopAllCoroutines(); 
                StartCoroutine(RecallRoutine(targetCog));
            }
        }

        private IEnumerator RecallRoutine(Cogs cog)
        {
            var rb = cog.GetComponent<Rigidbody2D>();
           
            var currentSocket = cog.GetComponentInParent<Socket>();
            if (currentSocket != null) 
            {
                currentSocket.UnsnapCog();
            }

            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;

            while (Vector3.Distance(cog.transform.position, transform.position) > 0.05f)
            {
                cog.transform.position = Vector3.MoveTowards(
                    cog.transform.position,
                    transform.position,
                    flySpeed * Time.deltaTime
                );
                yield return null;
            }

    
            SnapCog(cog.gameObject, cog);
        }

        private Cogs FindTargetCog()
        {
            Cogs[] allCogs = Object.FindObjectsByType<Cogs>(FindObjectsSortMode.None);
            foreach (var c in allCogs)
            {
                foreach (var allowed in allowedTypes)
                {
                    if (c.cogType == allowed) return c;
                }
            }
            return null;
        }
    }
}