using System.Collections;
using UnityEngine;

namespace Team3
{
    public class AttackItem : MonoBehaviour
    {
        [SerializeField] private float cooldown = 10f;
        private SpriteRenderer spriteRenderer;
        private bool isActive;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive) return;
            if (!other.CompareTag("Player")) return;

            DamageItemState state = other.GetComponent<DamageItemState>();
            if (state == null) return;

            state.Activate();
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            isActive = true;
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(cooldown);

            spriteRenderer.enabled = true;
            isActive = false;
        }
    }
}
