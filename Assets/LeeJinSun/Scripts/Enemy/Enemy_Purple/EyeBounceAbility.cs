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

        //상태 제어를 위한 변수들
        private bool isAbilityActive = false;               //능력이 작동 중인가?
        private bool isBouncing = false;                    //실제로 튕겨다니는 중인가?
        private Vector2 bounceDirection;
        private Camera mainCam;
        private Vector3 originalPosition;                    // 패턴 시작 전 위치 저장용
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
            isAbilityActive = false;
            isBouncing = false;

            if (eyeVisual != null) eyeVisual.SetActive(true);
            if (gameManager != null) gameManager.OnGameOver += HandleGameOver;

            StartCoroutine(EyeRoutine());
        }

        public void OnExit()
        {
            StopAllCoroutines();
            isAbilityActive = false;
            isBouncing = false;
            if (gameManager != null) gameManager.OnGameOver -= HandleGameOver;
            if (eyeVisual != null) eyeVisual.SetActive(false);
            //SetBouncingState(false); // 종료 시 추적 모드로 복구
        }

        public void OnTick()
        {
            if (!isAbilityActive) return; // 평소(추적 중)에는 아무것도 안 함

            // 튕기기 모드일 때만 별도의 이동 로직 실행
            if (isBouncing)
            {
                BounceMovement();
            }
            else
            {
                // [해결 3] 준비 애니메이션 중에는 위치를 고정시켜서 이동을 막음
                // Enemy_Main이 Update에서 이동시키더라도 여기서 강제로 멈춤
                owner.transform.position = owner.transform.position;
            }
        }

        public void OnGameOver() => OnExit();

        private IEnumerator EyeRoutine()
        {
            while (true)
            {
                // 1. 일반 추적 상태
                isAbilityActive = false;
                isBouncing = false;
                // 복귀 시 회전 초기화 (똑바로 서게 함)
                owner.transform.rotation = Quaternion.identity;

                //if (owner != null) owner.enabled = true;
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                originalPosition = owner.transform.position;

                // 2. 점프 준비 (예고 동작)
                // 바로 튕기면 플레이어가 대응하기 힘드므로, 준비 애니메이션을 먼저 보여줍니다.
                isAbilityActive = true;
                if (animator != null) animator.SetTrigger("IsJump");
                yield return new WaitForSeconds(1f);

                // 3. 튕기기 상태로 전환
                animator.SetBool("IsBounce", true);

                // [중요] SetBouncingState 내부의 랜덤 방향 로직이 실행되기 전에 
                // 방향을 강제로 위(Vector2.up)로 고정합니다.
                isBouncing = true;
                bounceDirection = Vector2.up;

                //SetBouncingState(true);

               /* // 플레이어 반대 방향으로 초기 방향 설정
                Vector3 playerPos = owner.player.transform.position;
                bounceDirection = (owner.transform.position - playerPos).normalized;*/

                // 튕기는 지속 시간 동안 대기
                yield return new WaitForSeconds(bounceDuration);

                // 4. 튕기기 종료
                isBouncing = false;
                animator.SetBool("IsBounce", false);

                /*// [해결 2] 튕기기 종료 후 플레이어와 너무 가까우면 뒤로 살짝 밀어냄
                float dist = Vector2.Distance(owner.transform.position, playerPos);
                if (dist < 3f)
                {
                    Vector3 pushDir = (owner.transform.position - playerPos).normalized;
                    owner.transform.position += pushDir * 2f;
                }*/

                owner.transform.position = originalPosition;
                owner.transform.rotation = Quaternion.identity; // 회전 초기화

                ReturnToOriginalPosition();

                // 잠시 숨을 고른 뒤 다시 루프 시작
                yield return new WaitForSeconds(1f);

                isAbilityActive = false;
            }
        }

        private void ReturnToOriginalPosition()
        {
            // 방법 1: 즉시 순간이동 (안전함)
            owner.transform.position = originalPosition;

            // 방법 2: 만약 복귀 애니메이션을 보여주며 부드럽게 이동하고 싶다면 
            // 여기서 별도의 Lerp 코루틴을 돌릴 수도 있습니다. 
            // 현재는 논리적 오류를 줄이기 위해 순간이동 방식을 추천합니다.

            Debug.Log("원래 위치로 복귀 완료!");
        }

        private void SetBouncingState(bool bouncing)
        {
            isBouncing = bouncing;

            /*if (isBouncing)
            {
                // [핵심] 튕기는 동안 본체의 추격 로직을 완전히 끕니다.
                // 이게 켜져 있으면 튕기면서도 계속 플레이어 쪽으로 몸이 쏠립니다.
                //if (owner != null) owner.enabled = false;

                // [플레이어 회피 로직]
                // 초기 방향 설정: 플레이어 반대 방향
                if (owner.player != null)
                {
                    Vector3 playerPos = owner.player.transform.position;
                    Vector3 awayFromPlayer = (owner.transform.position - playerPos).normalized;

                    // 약간의 랜덤성 추가 (완전 직선이 아니게)
                    float randomAngle = Random.Range(-30f, 30f);
                    bounceDirection = Quaternion.Euler(0, 0, randomAngle) * awayFromPlayer;
                }
                else
                {
                    // 플레이어를 못 찾을 경우 랜덤 방향
                    bounceDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                }
            }*/
        }

        private void BounceMovement()
        {
            // 위치 이동
            owner.transform.position += (Vector3)(bounceDirection * bounceSpeed * Time.deltaTime);

            // [추가] 2. 이동 방향을 바라보도록 회전 (자연스러운 방향 전환)
            if (bounceDirection != Vector2.zero)
            {
                // Vector2.up(0, 1)이 기본 정면일 때를 기준으로 각도 계산
                float angle = Mathf.Atan2(bounceDirection.y, bounceDirection.x) * Mathf.Rad2Deg;

                // 눈알의 기본 이미지가 위를 보고 있다면 -90도, 오른쪽을 보고 있다면 0도 등으로 보정 필요
                // 아래는 눈알이 오른쪽(Right)을 바라보는 이미지일 때 기준입니다.
                owner.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            // 화면 경계 체크 (Viewport: 0~1 사이 값)
            Vector3 viewPos = mainCam.WorldToViewportPoint(owner.transform.position);

            bool hitWall = false;

            // 좌우 벽 충돌
            if (viewPos.x <= 0.05f)
            {
                bounceDirection.x = Mathf.Abs(bounceDirection.x); // 오른쪽으로 강제 반사
                hitWall = true;
            }
            else if (viewPos.x >= 0.95f)
            {
                bounceDirection.x = -Mathf.Abs(bounceDirection.x); // 왼쪽으로 강제 반사
                hitWall = true;
            }

            // 위쪽 벽(천장) 충돌 시 - 여기서 사방으로 퍼지는 랜덤각도 부여
            if (viewPos.y >= 0.95f)
            {
                bounceDirection.y = -Mathf.Abs(bounceDirection.y); // 아래로 반사

                // [중요] 천장에 처음 부딪혔을 때 수직으로만 움직이지 않게 좌우 랜덤값 추가
                if (Mathf.Abs(bounceDirection.x) < 0.1f)
                {
                    bounceDirection.x = Random.Range(-1f, 1f);
                    bounceDirection = bounceDirection.normalized;
                }
                hitWall = true;
            }
            // 아래쪽 벽 충돌
            else if (viewPos.y <= 0.05f)
            {
                bounceDirection.y = Mathf.Abs(bounceDirection.y);
                hitWall = true;
            }

            // 3. 벽에 부딪혔을 때 위치 보정 (끼임 방지)
            if (hitWall)
            {
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
