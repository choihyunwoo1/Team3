using System.Collections;
using UnityEngine;

namespace Team3
{
    public class LaserTurret : MonoBehaviour
    {
        [Header("Damage")]
        public int minDamage = 3;
        public int maxDamage = 10;
        public float normalDuration = 3f;
        public float buffDuration = 5f;
        public float cooldown = 3f;

        [Header("Refs")]
        public LineRenderer line;
        public GameObject endParticle;
        public MiniGameEnemy enemy;

        private bool canUse = true;

        private void Awake()
        {
            if (line != null)
                line.enabled = false;

            if (endParticle != null)
                endParticle.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canUse) return;
            if (!other.CompareTag("Player")) return;

            float duration = normalDuration;

            // ✅ DamageBuff가 "켜져 있으면" 지속시간 증가
            DamageBuff buff = other.GetComponent<DamageBuff>();
            if (buff != null && buff.enabled)
            {
                duration = buffDuration;
            }

            StartCoroutine(FireLaser(duration));
        }

        private IEnumerator FireLaser(float duration)
        {
            canUse = false;

            if (line != null)
                line.enabled = true;

            if (endParticle != null)
                endParticle.SetActive(true);

            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;

                float t = Mathf.Clamp01(time / duration);
                int damage = Mathf.RoundToInt(
                    Mathf.Lerp(minDamage, maxDamage, t)
                );

                if (enemy != null)
                    enemy.TakeDamage(damage);

                yield return null;
            }

            if (line != null)
                line.enabled = false;

            if (endParticle != null)
                endParticle.SetActive(false);

            yield return new WaitForSeconds(cooldown);
            canUse = true;
        }
    }
}
