using UnityEngine;

namespace Choi
{
    public class Player : MonoBehaviour
    {
        [Header("점프 설정")]
        public float jumpForce = 7f;
        private int jumpCount = 0;
        private int maxJumpCount = 2;

        [Header("이동 설정")]
        public float moveSpeed = 5f;

        private Rigidbody2D rb2D;
        private AudioSource audioSource;

        private bool isGrounded = false;

        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();

            // 최신 물리엔진에서 Sleep 방지
            rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        void Update()
        {
            InputJump();
        }

        void FixedUpdate()
        {
            if (!GameManager.IsDeath)
                MoveForward();
        }

        // ★ Update에서 입력만 감지
        void InputJump()
        {
            if (GameManager.IsDeath) return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                TryJump();
            }
        }

        // ★ Rigidbody로 점프
        void TryJump()
        {
            if (!isGrounded && jumpCount >= maxJumpCount) return;

            // 점프 초기화
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0f);

            // 위로 힘 주기
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpCount++;
            isGrounded = false;
        }

        // ★ Rigidbody 이동 (물리적으로 자연스러움)
        void MoveForward()
        {
            rb2D.linearVelocity = new Vector2(moveSpeed, rb2D.linearVelocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                jumpCount = 0;
            }
            else if (collision.collider.CompareTag("Obstacle"))
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            GameManager.IsDeath = true;
            rb2D.linearVelocity = Vector2.zero;  // 즉시 멈춤
                                                 
            rb2D.bodyType = RigidbodyType2D.Kinematic; // 충돌되고 벽에 끼는 거 방지

            // 필요하면 애니메이션이나 SFX 추가 가능
            Debug.Log("Player Died");
        }
    }
}
