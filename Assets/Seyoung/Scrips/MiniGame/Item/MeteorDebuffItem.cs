using UnityEngine;

namespace Team3
{
    public class MeteorDebuffItem : MonoBehaviour
    {
        public float speedMultiplier = 0.5f; // 속도 50%
        public float scaleMultiplier = 0.6f; // 크기 60%

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            SpawnManager spawnManager = FindAnyObjectByType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.meteorSpeed *= speedMultiplier;
                spawnManager.meteorScale *= scaleMultiplier;
            }

            Destroy(gameObject);
        }
    }
}
