using UnityEngine;

namespace Team3
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private float fallSpeed = 5f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // ⭐ SpawnManager에서 호출
        public void Init(float speed, float scale)
        {
            fallSpeed = speed;
            transform.localScale = Vector3.one * scale;
        }

        private void Start()
        {
            rb.linearVelocity = Vector2.down * fallSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerMove playerMove = other.gameObject.GetComponent<PlayerMove>();
                if (playerMove != null)
                    playerMove.Die();

                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}
