using UnityEngine;

namespace Team3
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private float fallSpeed = 5f;

        [Header("Impact Effect")]
        [SerializeField] private GameObject impactEffectPrefab;
        public Transform firePoint;

        private Rigidbody2D rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

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

                SpawnImpactEffect();
                Destroy(gameObject,1f);
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                SpawnImpactEffect();
                Destroy(gameObject, 1f);
            }
        }

        // ⭐ 이펙트 생성 전용 메서드
        private void SpawnImpactEffect()
        {
            if (impactEffectPrefab == null)
                return;

            // ⭐ firePoint가 있으면 거기서, 없으면 메테오 위치
            Vector3 spawnPos = firePoint != null
                ? firePoint.position
                : transform.position;

            GameObject effect = Instantiate(
                impactEffectPrefab,
                spawnPos,
                Quaternion.identity
            );

            Destroy(effect, 2f);
        }

    }
}

