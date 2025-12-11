using UnityEngine;

namespace Choi
{
    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("점프 설정")]
        public float jumpForce = 7f;
        private int jumpCount = 0;
        private int maxJumpCount = 2;

        [Header("이동 설정")]
        public float moveSpeed = 5f;

        private Rigidbody2D rb2D;
        private AudioSource audioSource;

        public Collider2D groundCollider;
        public Collider2D frontCollider;

        private bool isFrontBlocked = false;
        private bool isGrounded = false;
        #endregion

        #region Unity Event Method
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
            if (GameManager.State != GameState.Playing)
                return;

            MoveForward();
        }
        #endregion

        #region Custom Method
        // Update에서 입력만 감지
        void InputJump()
        {
            if (GameManager.State != GameState.Playing)
                return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                TryJump();
            }
        }

        // Rigidbody로 점프
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

        // Rigidbody 이동 (물리적으로 자연스러움)
        void MoveForward()
        {
            // 공중 + 앞 막힘 → 이동 중지
            if (!isGrounded && isFrontBlocked)
            {
                rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
                return;
            }

            // 정상 이동
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

            // 물리 정지
            rb2D.linearVelocity = Vector2.zero;
            rb2D.bodyType = RigidbodyType2D.Kinematic;

            // GameManager에 GameOver 상태 전달
            GameManager.SetState(GameState.GameOver);

            Debug.Log("Player Died");
        }
        public void SetGrounded(bool grounded)
        {
            isGrounded = grounded;
            if (grounded) jumpCount = 0;
        }

        public void SetFrontBlocked(bool blocked)
        {
            isFrontBlocked = blocked;
        }
        #endregion
    }
}
