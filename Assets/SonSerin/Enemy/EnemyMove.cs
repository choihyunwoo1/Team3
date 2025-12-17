using UnityEngine;

namespace JS
{
    public class EnemyMove : MonoBehaviour
    {
        #region Variables

        public float speed = 3f;           // 플레이어를 쫓는 속도
        private Transform player;          // 플레이어 트랜스폼

        [SerializeField] private float floatAmplitude = 0.3f;
        [SerializeField] private float floatFrequency = 3f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (GameManager.State != GameState.Playing) return;

            //FollowPlayer();
            FollowPlayerGhostStyle();
            CatchUpIfTooFar();
        }

        // 트리거 충돌 처리
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.SetState(GameState.GameOver);
                Debug.Log("Enemy caught the Player!");
            }
        }
        #endregion

        #region Custom Method
        private void FollowPlayer()
        {
            // 플레이어를 향해 x축만 따라가기
            Vector3 targetPos = new Vector3(player.position.x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
        }

        private void CatchUpIfTooFar()
        {
            // Enemy가 너무 뒤쳐졌으면 순간이동
            if (player.position.x - transform.position.x > 10f)
            {
                transform.position = new Vector3(
                    player.position.x - 8f,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
        private void FollowPlayerGhostStyle()
        {
            // 부유하는 Y 오프셋 그대로 사용
            float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

            // Y방향은 플레이어 쪽으로 더 강하게 당기기
            float yTarget = Mathf.Lerp(transform.position.y, player.position.y, 0.15f);

            // 약간의 부유효과 추가
            yTarget += offsetY;

            // X는 많이 따라오면 바로 잡아버리므로 느리게
            float xTarget = Mathf.Lerp(transform.position.x, player.position.x, 0.02f);

            Vector3 target = new Vector3(
                xTarget,
                yTarget,
                transform.position.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                target,
                speed * Time.deltaTime   // 전체 이동 부드럽게
            );
        }
        #endregion
    }
}