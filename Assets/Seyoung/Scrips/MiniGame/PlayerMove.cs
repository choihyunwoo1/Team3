using UnityEngine;

namespace Team3
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Move")]
        public float moveSpeed = 5f;

        [Header("Jump")]
        public float jumpForce = 12f;
        public Transform groundCheck;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;

        private Rigidbody2D rb;
        private float moveInput;
        private bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // 좌우 이동 (A, D)
            moveInput = Input.GetAxisRaw("Horizontal");

            // 바닥 체크
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

            // 점프 (W 또는 Space)
            if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }
}