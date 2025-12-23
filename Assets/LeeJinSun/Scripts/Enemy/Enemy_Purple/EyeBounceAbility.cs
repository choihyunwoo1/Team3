using UnityEngine;
using System.Collections;

namespace JS
{
    public class EyeBounceAbility : MonoBehaviour, IEnemyAbility
    {
        #region Variables
        private Enemy_Main owner;
        private GameManager gameManager;
        [SerializeField] Animator animator;
 
        [Header("Visuals")]
        [SerializeField] private GameObject eyeVisual;

        [Header("Bounce Settings")]
        [SerializeField] private float bounceSpeed = 12f;   // 튕길 때 속도
        [SerializeField] private float bounceDuration = 5f; // 튕기는 지속 시간

        [Header("Interval")]
        [SerializeField] private float minWaitTime = 3f;    // 추적하며 기어가는 최소 시간
        [SerializeField] private float maxWaitTime = 6f;    // 추적하며 기어가는 최대 시간

        private bool isBouncing = false;
        private Vector2 bounceDirection;
        private Camera mainCam;
        #endregion

        #region Custom Method
        public void Setup(Enemy_Main enemy)
        {
            owner = enemy;
            gameManager = Object.FindAnyObjectByType<GameManager>();
            mainCam = Camera.main;
            animator = eyeVisual.GetComponent<Animator>();
        }

        public void OnEnter()
        {
            StopAllCoroutines();
            SetBouncingState(false); // 시작은 추적 모드
            if (eyeVisual != null) eyeVisual.SetActive(true);

            if (gameManager != null) gameManager.OnGameOver += HandleGameOver;

            StartCoroutine(EyeRoutine());
        }

        public void OnExit()
        {
            StopAllCoroutines();
            if (gameManager != null) gameManager.OnGameOver -= HandleGameOver;
            if (eyeVisual != null) eyeVisual.SetActive(false);
            SetBouncingState(false); // 종료 시 추적 모드로 복구
        }

        public void OnTick()
        {
            // 튕기기 모드일 때만 별도의 이동 로직 실행
            if (isBouncing)
            {
                BounceMovement();
            }
        }

        public void OnGameOver() => OnExit();

        private IEnumerator EyeRoutine()
        {
            while (true)
            {
                // 1. 일반 추적 상태 (Enemy_Main의 로직이 작동함)
                SetBouncingState(false);
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                animator.SetTrigger("IsJump");
                yield return new WaitForSeconds(2f);


                // 2. 튕기기 상태로 전환 (Enemy_Main의 로직을 잠시 멈춤)
                SetBouncingState(true);
                animator.SetTrigger("IsBounce");
                yield return new WaitForSeconds(bounceDuration);
            }
        }

        private void SetBouncingState(bool bouncing)
        {
            isBouncing = bouncing;

            if (isBouncing)
            {
                // 튕기기 시작할 때 Enemy_Main의 기본 추적 기능을 멈춥니다.
                // 만약 Enemy_Main에 추적 활성화/비활성화 변수가 있다면 여기서 조절

                // 초기 튕김 방향 설정
                float randomX = Random.value > 0.5f ? 1f : -1f;
                float randomY = Random.value > 0.5f ? 1f : -1f;
                bounceDirection = new Vector2(randomX, randomY).normalized;
            }
            else
            {
                // 다시 추적 모드로 돌아갈 때 Enemy_Main 기능을 켭니다.
                // 예: owner.isTracking = true;
            }
        }

        private void BounceMovement()
        {
            // 튕기기 로직 (이동)
            owner.transform.Translate(bounceDirection * bounceSpeed * Time.deltaTime);

            // 화면 경계 체크
            Vector3 viewPos = mainCam.WorldToViewportPoint(owner.transform.position);

            if (viewPos.x <= 0.05f || viewPos.x >= 0.95f)
            {
                bounceDirection.x *= -1;
                ClampPosition();
            }
            if (viewPos.y <= 0.05f || viewPos.y >= 0.95f)
            {
                bounceDirection.y *= -1;
                ClampPosition();
            }
        }

        private void ClampPosition()
        {
            Vector3 pos = mainCam.WorldToViewportPoint(owner.transform.position);
            pos.x = Mathf.Clamp(pos.x, 0.06f, 0.94f);
            pos.y = Mathf.Clamp(pos.y, 0.06f, 0.94f);
            owner.transform.position = mainCam.ViewportToWorldPoint(pos);
        }

        private void HandleGameOver(DeathCause cause) => OnGameOver();
        #endregion
    }
}
