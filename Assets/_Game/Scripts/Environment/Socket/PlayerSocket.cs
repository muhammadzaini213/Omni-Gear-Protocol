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
            // WAJIB: Agar logika Unsnap saat gir di-drag tetap jalan
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
                // Jika sudah menempel di socket ini, abaikan
                if (targetCog.transform.parent == transform) return;

                // Stop coroutine sebelumnya jika ada (mencegah double recall)
                StopAllCoroutines(); 
                StartCoroutine(RecallRoutine(targetCog));
            }
        }

        private IEnumerator RecallRoutine(Cogs cog)
        {
            var rb = cog.GetComponent<Rigidbody2D>();
            
            // 1. Unsnap dari socket mana pun dia berada sekarang
            // Ini akan memicu Gate_Close/Elevator_Reset secara otomatis
            var currentSocket = cog.GetComponentInParent<Socket>();
            if (currentSocket != null) 
            {
                currentSocket.UnsnapCog();
            }

            // 2. Siapkan physics untuk terbang (Kinematic agar tidak tabrakan)
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;

            // 3. Proses terbang balik
            while (Vector3.Distance(cog.transform.position, transform.position) > 0.05f)
            {
                cog.transform.position = Vector3.MoveTowards(
                    cog.transform.position,
                    transform.position,
                    flySpeed * Time.deltaTime
                );
                yield return null;
            }

            // 4. WAJIB: Kunci gir ke socket player
            // Memanggil fungsi dari parent agar status _isCogSnapped jadi true
            SnapCog(cog.gameObject, cog);
        }

        private Cogs FindTargetCog()
        {
            // Mencari gir yang tipenya cocok dengan allowedTypes socket ini
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