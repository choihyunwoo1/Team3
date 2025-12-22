using UnityEngine;
using System.Collections;

namespace JS
{
    /// <summary>
    /// Enemy 외형: 슬라임, 능력: 플레이어 앞길 화면 막기
    /// </summary>
    public class SlimeAbility : MonoBehaviour, IEnemyAbility
    {
        #region Variables
        private Enemy_Main owner; // Enemy 본체 참조
        private GameManager gameManager;

        [Header("Visuals")]
        [SerializeField] private GameObject slimeVisual;        // 자식으로 넣은 '슬라임 모양' 오브젝트

        [Header("Slime Settings")]
        [SerializeField] private GameObject slimeScreen;            //화면 가릴 슬라임

        [Header("Cycle Settings")]
        [SerializeField] private float minWaitTime = 2f;
        [SerializeField] private float maxWaitTime = 4f;
        [SerializeField] private float spitDelay = 0.5f; // 애니메이션 상 슬라임이 입에서 나가는 타이밍

        private float slimeTimer;
        #endregion

        #region Custom Method
        // 1. 초기 설정: Enemy 본체가 자신을 등록할 때 호출
        public void Setup(Enemy_Main enemy)
        {
            owner = enemy;
            gameManager = FindAnyObjectByType<GameManager>();
        }

        // 2. 능력 시작: 외형을 바꾸고 타이머 초기화
        public void OnEnter()
        {
            //혹시 진행되는 코루틴들 멈추게 하기
            StopAllCoroutines();

            if(gameManager != null)
            {
                gameManager.OnGameOver += HandleGameOver;
            }

            //슬라임 코루틴 시작
            StartCoroutine(SlimeSpitRoutine());
            owner.speed *= 0.5f;
            Debug.Log("SlimeAbility: 루틴 시작");
        }

        // 3. 실행: Enemy의 Update에서 매 프레임 호출됨
        public void OnTick()
        {

        }

        // 4. 능력 종료: 외형을 끄고 상태 정리
        public void OnExit()
        {
            StopAllCoroutines();
            owner.speed *= 2f;

            if (gameManager != null)
            {
                gameManager.OnGameOver -= HandleGameOver;
            }

            if (slimeVisual != null)
            {
                slimeVisual.SetActive(false);
            }
            slimeScreen.gameObject.SetActive(false);

        }

        private void HandleGameOver(DeathCause cause)
        {
            if (slimeScreen != null)
            {
                slimeScreen.gameObject.SetActive(false);
            }
        }

        private IEnumerator SlimeSpitRoutine()
        {
            while (true)
            {
                // 1. 대기 단계 (슬라임 모드 외형 유지)
                //animator.SetBool("isSlimeMode", true);
                if (slimeVisual != null)
                {
                    slimeVisual.SetActive(true);
                }

                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                // 2. 발사 전조 (입을 크게 벌리는 애니메이션 등)
                //animator.SetTrigger("doSpit");
                yield return new WaitForSeconds(spitDelay);

                // 3. 실제 UI 가리기 실행
                if (slimeScreen != null)
                {
                    slimeScreen.SetActive(true);
                    Debug.Log("슬라임 발사! 화면을 가립니다.");
                }
            }
        }

        #endregion
    }
}
