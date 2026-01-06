using System.Collections;
using UnityEngine;

namespace Team3
{
    public class Dynamite : MonoBehaviour
    {
        [Header("Damage")]
        public int damage = 10;
        public float cooldown = 3f;
        public float dropInterval = 0.25f;

        [Header("References")]
        public MiniGameEnemy enemy;
        public GameObject particle;

        private bool isRunning;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (particle != null)
                particle.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isRunning) return;
            if (!other.CompareTag("Player")) return;

            StartCoroutine(TriggerDamage());
        }

        private IEnumerator TriggerDamage()
        {
            isRunning = true;

            // 시각 효과
            spriteRenderer.enabled = false;
            if (particle != null)
                particle.SetActive(true);

            // 데미지 1회
            if (enemy != null)
                enemy.TakeDamage(damage);

            yield return new WaitForSeconds(cooldown);

            // 복구
            spriteRenderer.enabled = true;
            if (particle != null)
                particle.SetActive(false);

            isRunning = false;
        }
    }
    
}
