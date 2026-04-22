using UnityEngine;

namespace _Game.Scripts.Cogs
{
    public class Cogs : MonoBehaviour
    {
        public CogsType cogType;
        public float rotateSpeed;
        public bool reverseDirection;
        public bool isSnapped { get; private set; }

        [Header("Audio Settings")]
        [SerializeField] private AudioClip impactSFX;
        [SerializeField] private float minImpactForce = 2f; // Kekuatan minimum untuk bunyi

        void Update() => RotateCogs();

        public void RotateCogs()
        {
            if (!isSnapped) return;

            float direction = reverseDirection ? -1f : 1f;
            transform.Rotate(0f, 0f, direction * rotateSpeed * Time.deltaTime);
        }

        public void Snap() => isSnapped = true;

        public void UnsnapNotify() => isSnapped = false;

        // --- LOGIKA SUARA BENTURAN ---
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Jangan bunyikan suara jika sedang menempel di robot (Snapped)
            if (isSnapped) return;

            // Cek seberapa keras benturannya
            if (collision.relativeVelocity.magnitude > minImpactForce)
            {
                // Gunakan PlayEnvironmentSfx agar tidak memotong suara Player
                // Volume dinamis berdasarkan kekuatan benturan (opsional)
                float impactVol = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10f);
                SfxPlayer.Instance.PlayEnvironmentSfx(impactSFX, impactVol);
            }
        }
    }

    public enum CogsType { Small, Medium, Large }
}