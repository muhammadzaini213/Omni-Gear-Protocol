using UnityEngine;
using System.Collections;

namespace _Game.Scripts.Cogs
{
    public class PlayerSocket : Socket
    {
        [Header("Recall Config")]
        [SerializeField] private float flySpeed = 15f;
        [SerializeField] private KeyCode recallKey = KeyCode.R;
        [SerializeField] private AudioClip recallSound;

        protected override void Update()
        {
            if (_isDisabled) return;
            base.Update();

            if (Input.GetKeyDown(recallKey)) TryRecall();
        }

        public void EjectAllCogsOnDeath()
        {
            StopAllCoroutines();

            Cogs targetCog = _currentCog;

            DisableSocket();

            if (targetCog != null)
            {
                GameObject cogObj = targetCog.gameObject;

                var dragScript = cogObj.GetComponent<CogsDrag>();
                if (dragScript != null) dragScript.enabled = false;

                Rigidbody2D rb = cogObj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.isKinematic = false;
                    rb.simulated = true;
                    rb.gravityScale = 1.5f;

                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(1.2f, 2f)).normalized;
                    float randomForce = Random.Range(10f, 15f);
                    float randomTorque = Random.Range(-20f, 20f);

                    rb.velocity = Vector2.zero; // Reset velocity lama
                    rb.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);
                    rb.AddTorque(randomTorque, ForceMode2D.Impulse);
                }
                
                cogObj.tag = "Untagged";
            }
        }

        private void TryRecall()
        {
            Cogs targetCog = FindTargetCog();
            if (targetCog != null && targetCog.transform.parent != transform)
            {
                StopAllCoroutines();
                StartCoroutine(RecallRoutine(targetCog));

                if (recallSound != null)
                {
                    SfxPlayer.Instance.PlayPlayerSfx(recallSound);
                }   
            }
        }

        private IEnumerator RecallRoutine(Cogs cog)
        {
            var rb = cog.GetComponent<Rigidbody2D>();
            var col = cog.GetComponent<Collider2D>();
            var currentSocket = cog.GetComponentInParent<Socket>();

            if (currentSocket != null) currentSocket.UnsnapCog();
            if (col != null) col.enabled = false;

            cog.Snap();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;

            while (Vector3.Distance(cog.transform.position, transform.position) > 0.05f)
            {
                cog.transform.position = Vector3.MoveTowards(cog.transform.position, transform.position, flySpeed * Time.deltaTime);
                yield return null;
            }

            if (col != null) col.enabled = true;
            SnapCog(cog.gameObject, cog);
        }

        private Cogs FindTargetCog()
        {
            Cogs[] allCogs = Object.FindObjectsByType<Cogs>(FindObjectsSortMode.None);
            foreach (var c in allCogs)
            {
                foreach (var allowed in allowedTypes)
                    if (c.cogType == allowed) return c;
            }
            return null;
        }
    }
}