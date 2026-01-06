using System.Collections;
using UnityEngine;

namespace Team3
{
    public class DamageTrigger : MonoBehaviour
    {
        public int damage = 10;
        public float cooldown = 3f;
        public float repeatInterval = 0.25f;

        public MiniGameEnemy enemy;
        public Animator animator;
        public GameObject particle;

        private bool isRunning;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isRunning) return;
            if (!other.CompareTag("Player")) return;

            DamageItemState item = other.GetComponent<DamageItemState>();
            DamageBuff buff = other.GetComponent<DamageBuff>();

            // ❗ DamageItem 없으면 절대 작동 안 함
            if (item == null || !item.HasItem)
                return;

            int repeat = (buff != null && buff.HasBuff) ? 3 : 1;

            StartCoroutine(Explode(other, repeat));
        }

        private IEnumerator Explode(Collider2D player, int repeat)
        {
            isRunning = true;

            for (int i = 0; i < repeat; i++)
            {
                if (animator != null)
                    animator.SetTrigger("OnTrigger");

                if (particle != null)
                    particle.SetActive(true);

                if (enemy != null)
                    enemy.TakeDamage(damage);

                yield return new WaitForSeconds(repeatInterval);
            }

            if (particle != null)
                particle.SetActive(false);

            // ⭐ 사용 후 무조건 소모
            player.GetComponent<DamageItemState>()?.Consume();
            player.GetComponent<DamageBuff>()?.Consume();

            yield return new WaitForSeconds(cooldown);
            isRunning = false;
        }
    }
}
