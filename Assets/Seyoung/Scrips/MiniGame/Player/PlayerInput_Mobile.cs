using UnityEngine;

namespace Team3
{
    public class PlayerInput_Mobile : MonoBehaviour
    {
        public enum MoveDir { Left, Right }
        public MoveDir direction;

        [SerializeField] private PlayerMove player;
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D rb;
        private bool isPressed = false;

        private void Start()
        {
            if (player == null)
                player = FindAnyObjectByType<PlayerMove>();

            rb = player.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!isPressed) return;
            if (player.IsDead()) return;

            float dir = direction == MoveDir.Left ? -1f : 1f;

            rb.linearVelocity = new Vector2(
                dir * moveSpeed,
                rb.linearVelocity.y
            );
        }

        // 🔽 버튼 누름
        public void OnButtonDown()
        {
            isPressed = true;
        }

        // 🔼 버튼 뗌
        public void OnButtonUp()
        {
            isPressed = false;

            // X 이동 멈춤 (Y는 유지)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}
