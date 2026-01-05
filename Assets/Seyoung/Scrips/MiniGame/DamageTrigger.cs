using System.Collections;
using UnityEngine;

namespace Team3
{
    public class DamageTrigger : MonoBehaviour
    {
        public int damage = 10;
        public float cooldown = 3f;
        public MiniGameEnemy enemy;
        public GameObject particle;

        private Animator animator;
        private bool canDamage = true;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canDamage)
                return;

            if (!other.CompareTag("Player"))
                return;

            // 🔥 전역 아이템 상태 체크
            if (!AttackItem.IsItemActive)
                return;

            if (enemy != null)
                enemy.TakeDamage(damage);

            StartCoroutine(DamageCooldown());
        }

        private IEnumerator DamageCooldown()
        {
            canDamage = false;

            if (particle != null)
                particle.SetActive(true);

            if (animator != null)
                animator.SetTrigger("OnTrigger");

            yield return new WaitForSeconds(cooldown);

            canDamage = true;

            if (particle != null)
                particle.SetActive(false);
        }
    }
}
