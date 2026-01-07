using Choi;
using System;
using UnityEngine;

namespace Team3
{
    public class PlayerMove : MonoBehaviour
    {
        #region Variables
        [Header("점프 설정")]
        [SerializeField] private float jumpForce = 7f;
        [SerializeField] private int maxJumpCount = 2;
        private int jumpCount;

        [Header("이동 설정")]
        [SerializeField] private float moveSpeed = 5f;
        private float moveInput;

        [Header("Wall Check")]
        [SerializeField] private GameObject wallCheckLeft;
        [SerializeField] private GameObject wallCheckRight;

        private Rigidbody2D rb2D;
        private AudioSource audioSource;
        private float mobileMoveInput = 0f;


        [SerializeField] private bool isFrontBlocked;
        [SerializeField] private bool isGrounded;
        private bool isDead = false;
        private bool jumpPressed;

        [SerializeField] private MiniGameManager gameManager;
        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private int moveDirection = 1;

        public GameObject gameOverUI;

        public event Action<DeathCause> OnPlayerDied;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();

            rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        private void Update()
        {
            if (gameManager.State != GameState.Playing || isDead)
                return;


            // 좌우 이동 입력 (A / D)
            moveInput = Input.GetAxisRaw("Horizontal") + MobileInput.Horizontal;

            if (MobileInput.Jump)
            {
                jumpPressed = true;
            }

            // 점프 입력 (W / Space )
            if (Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.Space))
            {
                jumpPressed = true;
            }
        }

        private void FixedUpdate()
        {
            if (gameManager.State != GameState.Playing || isDead)
                return;


            MoveHorizontal();

            if (jumpPressed)
            {
                TryJump();
                jumpPressed = false;
            }
        }
        #endregion

        #region Custom Method
        private void TryJump()
        {
            if (jumpCount >= maxJumpCount)
                return;

            jumpCount++;
            isGrounded = false;

            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0f);
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void MoveHorizontal()
        {
            float finalInput = moveInput + mobileMoveInput;
            finalInput = Mathf.Clamp(finalInput, -1f, 1f);

            rb2D.linearVelocity = new Vector2(
                finalInput * moveSpeed,
                rb2D.linearVelocity.y
            );

            // 방향 갱신도 finalInput 기준
            if (finalInput > 0)
                moveDirection = 1;
            else if (finalInput < 0)
                moveDirection = -1;

        }

        public void ReverseDirection()
        {
            moveDirection *= -1;
            UpdateWallCheck();
            RecheckFrontBlocked();
        }

        private void UpdateWallCheck()
        {
            if (moveDirection > 0)
            {
                wallCheckRight.SetActive(true);
                wallCheckLeft.SetActive(false);
            }
            else
            {
                wallCheckRight.SetActive(false);
                wallCheckLeft.SetActive(true);
            }
        }

        private void RecheckFrontBlocked()
        {
            var activeCheck = moveDirection > 0 ? wallCheckRight : wallCheckLeft;
            var hit = Physics2D.OverlapCircle(
                activeCheck.transform.position,
                0.1f,
                LayerMask.GetMask("Wall")
            );

            SetFrontBlocked(hit != null);
        }

        public void Die()
        {
            if (isDead)
                return;

            isDead = true;
            gameOverUI.SetActive(true);

            Freeze();

            // 입력 플래그 초기화
            jumpPressed = false;
            moveInput = 0f;
        }

        public void SetGrounded(bool grounded)
        {
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
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public void SetMoveSpeed(float value)
        {
            moveSpeed = value;
        }
        public void SetSpeedMultiplier(float multiplier)
        {
            moveSpeed = 5f * multiplier;
        }
        public bool IsDead()
        {
            return isDead;
        }

        public void Freeze()
        {
            // 이동/점프 완전 정지
            rb2D.linearVelocity = Vector2.zero;      // 현재 속도 0
            rb2D.angularVelocity = 0f;         // 회전 속도 0
            rb2D.isKinematic = true;           // 물리 제어 끄기
        }
        public void SetMoveInput(float value)
        {
            moveInput = value;
        }

        public void Jump()
        {
            if (gameManager.State != GameState.Playing || isDead)
                return;

            jumpPressed = true;
        }
        // 모바일 버튼용
        public void MobileMove(float value)
        {
            mobileMoveInput = value;
        }

        public void MobileStop()
        {
            mobileMoveInput = 0f;
        }

        #endregion
    }
}
