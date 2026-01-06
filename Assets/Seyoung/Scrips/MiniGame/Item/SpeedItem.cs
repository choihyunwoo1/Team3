using System.Collections;
using UnityEngine;

namespace Team3
{
    public class SpeedItem : MonoBehaviour
    {
        [Header("Speed Settings")]
        [SerializeField] private float speedMultiplier = 2f;
        [SerializeField] private float duration = 3f;

        [Header("References")]
        [SerializeField] private GameObject speedUpText;

        private Collider2D col;
        private bool isActive = true;

        private void Awake()
        {
            col = GetComponent<Collider2D>();

            if (speedUpText != null)
                speedUpText.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if (!other.CompareTag("Player")) return;

            PlayerMove player = other.GetComponent<PlayerMove>();
            if (player == null) return;

            StartCoroutine(SpeedItemRoutine(player));
        }

        private IEnumerator SpeedItemRoutine(PlayerMove player)
        {
            isActive = false;
            col.enabled = false; // ⭐ 기능만 정지

            // 텍스트 표시
            if (speedUpText != null)
                speedUpText.SetActive(true);

            // 속도 증가
            player.SetSpeedMultiplier(speedMultiplier);

            yield return new WaitForSeconds(duration);

            // 원상복구
            player.SetSpeedMultiplier(1f);

            if (speedUpText != null)
                speedUpText.SetActive(false);

            col.enabled = true;
            isActive = true;
        }
    }
}
