using System.Collections;
using UnityEngine;

namespace Team3
{
    public class AttackItem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        // 🔥 전역 상태
        public static bool IsItemActive = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (IsItemActive)
                return;

            StartCoroutine(ItemEffect());
        }

        private IEnumerator ItemEffect()
        {
            IsItemActive = true;
            SetAllItemsVisible(false);

            yield return new WaitForSeconds(5f);

            IsItemActive = false;
            SetAllItemsVisible(true);
        }

        private void SetAllItemsVisible(bool value)
        {
            foreach (var item in FindObjectsOfType<AttackItem>())
            {
                item.spriteRenderer.enabled = value;
            }
        }
    }
}
