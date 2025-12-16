using UnityEngine;

namespace Choi
{
    public enum EnemyBuffType
    {
        None,
        SpeedUp,
        ScaleUp,
        LaserBeam,
    }
    public class Enemy : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float speed = 3f;
        [SerializeField] private float maxScale = 4f;

        private Transform player;

        [SerializeField] private float floatAmplitude = 0.3f;
        [SerializeField] private float floatFrequency = 3f;

        [SerializeField] private GameManager gameManager;
        [SerializeField] private CutsceneManager cutsceneManager;

        [Header("보스 원래 상태")]
        private float baseSpeed;
        private Vector3 baseScale;
        private EnemyBuffType currentBuff = EnemyBuffType.None;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //초기화
            baseSpeed = speed;
            baseScale = transform.localScale;
        }
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
                speed * Time.deltaTime
            );

            Vector3 target = new Vector3(xTarget, yTarget, transform.position.z);

            transform.position = Vector3.Lerp(
             transform.position,
             target,
             speed * Time.deltaTime
            );
        }
        public void ApplyBuff(EnemyBuffType type, float value)
        {
            if (currentBuff == type)
                return;

            RemoveCurrentBuff();

            switch (type)
            {
                case EnemyBuffType.SpeedUp:
                    speed *= value;
                    break;

                case EnemyBuffType.ScaleUp:
                    IncreaseScale(value);
                    break;
            }

            currentBuff = type;
        }
        private void RemoveCurrentBuff()
        {
            switch (currentBuff)
            {
                case EnemyBuffType.SpeedUp:
                    speed = baseSpeed;
                    break;

                case EnemyBuffType.ScaleUp:
                    transform.localScale = baseScale;
                    break;
            }

            currentBuff = EnemyBuffType.None;
        }
        public void IncreaseScale(float scaleMultiplier)
        {
            Vector3 newScale = transform.localScale * scaleMultiplier;

            if (newScale.x > maxScale)
                newScale = Vector3.one * maxScale;

            transform.localScale = newScale;
        }
        #endregion
    }
}
