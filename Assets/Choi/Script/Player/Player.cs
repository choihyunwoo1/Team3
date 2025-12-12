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

        [SerializeField]private bool isFrontBlocked = false;
        [SerializeField]private bool isGrounded = false;
        [SerializeField]private bool jumpPressed = false;
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                jumpPressed = true;
        }

        void FixedUpdate()
        {
            if (GameManager.State != GameState.Playing)
                return;

            MoveForward();

            if (jumpPressed)
            {
                TryJump();
                jumpPressed = false;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Obstacle"))
            {
                GameOver();
            }
        }

        #endregion

        #region Custom Method
        // Rigidbody로 점프
        void TryJump()
        {
            // jumpCount가 maxJumpCount (2) 이상이면 점프 금지 (0, 1은 허용)
            if (jumpCount >= maxJumpCount)
            {
                // 땅에 닿아 있으면 SetGrounded에서 jumpCount=0이 되었을 것이므로 이 조건문을 통과할 것입니다.
                return;
            }

            // 점프 실행 및 횟수 증가
            jumpCount++;
            isGrounded = false; // 점프하면 무조건 공중

            // 점프 초기화 및 힘 가하기
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0f);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Rigidbody 이동 (물리적으로 자연스러움)
        void MoveForward()
        {
            // 공중 + 앞이 막힘 → 이동 완전 정지
            if (!isGrounded && isFrontBlocked)
            {
                rb2D.linearVelocity = new Vector2(0f, rb2D.linearVelocity.y);
                return;
            }

            // 정상적인 전진 이동
            rb2D.linearVelocity = new Vector2(moveSpeed, rb2D.linearVelocity.y);
        }

        public void GameOver()
        {
            GameManager.IsDeath = true;

            // 물리 정지
            rb2D.linearVelocity = Vector2.zero;
            rb2D.bodyType = RigidbodyType2D.Kinematic;

            // GameManager에 GameOver 상태 전달
            GameManager.SetState(GameState.GameOver);
            Destroy(gameObject);

            Debug.Log("Player Died");
        }
        public void SetGrounded(bool grounded)
        {
            // 공중에서 Collider가 스쳐서 grounded 신호가 들어오더라도
            // 아래로 떨어지고 있을 때만 진짜 착지로 인정
            if (grounded && rb2D.linearVelocity.y <= 0.01f)
            {
                isGrounded = true;
                jumpCount = 0;
            }
            else if (!grounded)
            {
                isGrounded = false;
            }
        }

        public void SetFrontBlocked(bool blocked)
        {
            isFrontBlocked = blocked;
        }
        #endregion
    }
}
