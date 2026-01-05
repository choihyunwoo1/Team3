using System.Collections;
using Team3;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Tema3
{
    public class Dynamite : MonoBehaviour
    {
        public int damage = 10;
        public float cooldown = 3f;
        public MiniGameEnemy enemy;
        public GameObject particle;

        private bool canDamage = true;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canDamage)
                return;

            if (!other.CompareTag("Player"))
                return;

            if (enemy != null)
                enemy.TakeDamage(damage);

            StartCoroutine(DamageCooldown());
        }

        private IEnumerator DamageCooldown()
        {
            canDamage = false;
            spriteRenderer.enabled = false;
            particle.SetActive(true);

            yield return new WaitForSeconds(cooldown);

            canDamage = true;
            spriteRenderer.enabled = true;
            particle.SetActive(false);

        }
    }
}