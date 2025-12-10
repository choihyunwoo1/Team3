using UnityEngine;

namespace Choi
{
    public class Enemy : MonoBehaviour
    {
        #region Variables

        public float speed = 3f;           // 플레이어를 쫓는 속도
        private Transform player;          // 플레이어 트랜스폼
        #endregion

        #region Unity Event Method
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (GameManager.State != GameState.Playing) return;

            FollowPlayer();
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
        #endregion
    }
}