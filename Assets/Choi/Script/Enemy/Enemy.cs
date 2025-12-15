using UnityEngine;

namespace Choi
{
    public class Enemy : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float speed = 3f;

        private Transform player;

        [SerializeField] private float floatAmplitude = 0.3f;
        [SerializeField] private float floatFrequency = 3f;

        [SerializeField] private GameManager gameManager;
        [SerializeField] private CutsceneManager cutsceneManager;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Update()
        {
            if (gameManager.State != GameState.Playing)
                return;

            FollowPlayerGhostStyle();
            CatchUpIfTooFar();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 1. 충돌한 오브젝트에서 Player 컴포넌트 가져오기
            Player playerComponent = other.GetComponent<Player>();

            if (playerComponent == null)
                return;

            // 2. Player 인스턴스에서 Die 메서드 호출 및 DeathCause 전달
            // 적(Enemy)에 의한 사망이므로 DeathCause.EnemyA를 사용합니다.
            playerComponent.Die(DeathCause.EnemyA); // <--- 이 부분이 핵심!

            Debug.Log("Enemy caught the Player!");

            // (선택 사항: Enemy도 파괴 또는 비활성화 처리)
            gameObject.SetActive(false);
        }
        #endregion

        #region Custom Method
        private void CatchUpIfTooFar()
        {
            if (player == null)
                return;

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
            if (player == null)
                return;

            float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

            float yTarget = Mathf.Lerp(
                transform.position.y,
                player.position.y,
                0.15f
            ) + offsetY;

            float xTarget = Mathf.Lerp(
                transform.position.x,
                player.position.x,
                0.033f
            );

            Vector3 target = new Vector3(xTarget, yTarget, transform.position.z);

            transform.position = Vector3.Lerp(
                transform.position,
                target,
                0.1f
            );
        }
        #endregion
    }
}
