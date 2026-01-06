using System.Collections;
using UnityEngine;

namespace Team3
{
    public class LaserTurret : MonoBehaviour
    {
        [Header("Damage")]
        public int minDamage = 10;
        public int maxDamage = 100;
        public float duration = 3f;

        [Header("References")]
        public LineRenderer lineRenderer;
        public GameObject laserEndParticle;
        public MiniGameEnemy enemy;

        private bool isActivated = false;

        private void Awake()
        {
            lineRenderer.enabled = false;
            laserEndParticle.SetActive(false);

            // Inspector에서 만든 방향 그대로 사용
            lineRenderer.useWorldSpace = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (isActivated) return; // 이미 작동 중이면 무시

            StartCoroutine(LaserSequence());
        }

        private IEnumerator LaserSequence()
        {
            isActivated = true;

            lineRenderer.enabled = true;
            laserEndParticle.SetActive(true);

            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;

                float t = time / duration;
                int currentDamage = Mathf.RoundToInt(
                    Mathf.Lerp(minDamage, maxDamage, t)
                );

                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage);
                }

                yield return null;
            }

            // 종료
            lineRenderer.enabled = false;
            laserEndParticle.SetActive(false);

            isActivated = false;
        }
    }
}
