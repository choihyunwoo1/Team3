using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

namespace JS
{
    /// <summary>
    /// 마스크 웃음 공격(카메라 쉐이크)
    /// </summary>
    public class LaughAbility : MonoBehaviour, IEnemyAbility
    {
        #region Variables
        private Enemy_Main owner; // Enemy 본체 참조
        private GameManager gameManager;

        [Header("Visual")]
        [SerializeField] private GameObject maskVisual;        // 자식으로 넣은 '마스크 모양' 오브젝트

        [Header("Cinemachine Settings")]
        [SerializeField] private CinemachineCamera vCam; // 하이어라키의 CinemachineCamera 연결
        private CinemachineBasicMultiChannelPerlin noiseModule;

        [Header("Random Interval")]
        [SerializeField] private float minWaitTime = 3f;
        [SerializeField] private float maxWaitTime = 7f;

        [Header("Random Shake")]
        [SerializeField] private float minShakeDuration = 0.5f;
        [SerializeField] private float maxShakeDuration = 1.2f;
        [SerializeField] private float shakeAmplitude = 1.5f; // 흔들림 강도 (Amplitude)
        [SerializeField] private float shakeFrequency = 2.0f;  // 흔들림 속도 (Frequency)

        #endregion

        #region Custom Method
        // 1. 초기 설정: Enemy 본체가 자신을 등록할 때 호출
        public void Setup(Enemy_Main enemy)
        {
            owner = enemy;
            gameManager = Object.FindAnyObjectByType<GameManager>();

            // 시네머신 카메라에서 노이즈 모듈(Perlin)을 가져옵니다.
            if (vCam != null)
            {
                noiseModule = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        // 2. 능력 시작: 외형을 바꾸고 타이머 초기화
        public void OnEnter()
        {
            // 1. 기존 동작 정지 및 초기화
            StopAllCoroutines();
            ResetShake();

            // 2. 외형 변경 (자식 오브젝트 활성화 시 콜라이더도 자동 활성화)
            if (maskVisual != null) maskVisual.SetActive(true);

            // 3. 게임오버 이벤트 구독 (GameManager 스크립트 기반)
            if (gameManager != null)
            {
                gameManager.OnGameOver += HandleGameOver;
            }

            // 4. 웃음 루틴 시작
            StartCoroutine(LaughRoutine());
        }

        // 3. 실행: Enemy의 Update에서 매 프레임 호출됨
        public void OnTick()
        {

        }

        // 4. 능력 종료: 외형을 끄고 상태 정리
        public void OnExit()
        {
            // 1. 이벤트 구독 해제 (메모리 누수 방지)
            if (gameManager != null)
            {
                gameManager.OnGameOver -= HandleGameOver;
            }

            // 2. 모든 동작 정지 및 리셋
            StopAllCoroutines();
            if (maskVisual != null) maskVisual.SetActive(false);
            ResetShake();
        }

        public void OnGameOver()
        {
            // 인터페이스에 추가된 게임오버 대응 함수
            OnExit();
        }


        private IEnumerator LaughRoutine()
        {
            while (true)
            {
                // [랜덤 대기] 다음 웃음까지 기다림
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                // [랜덤 쉐이크] 흔들림 지속 시간 결정
                float shakeDuration = Random.Range(minShakeDuration, maxShakeDuration);

                // 흔들림 시작
                SetShake(shakeAmplitude, shakeFrequency);
                Debug.Log($"이너미 웃음 발동! 지속 시간: {shakeDuration:F1}초");

                // 흔들림 유지
                yield return new WaitForSeconds(shakeDuration);

                // 흔들림 정지
                ResetShake();
            }
        }

        //시네머신 노이즈 값을 설정해 화면 흔들기
        private void SetShake(float amplitude, float frequency)
        {
            if (noiseModule != null)
            {
                noiseModule.AmplitudeGain = amplitude;
                noiseModule.FrequencyGain = frequency;
            }
        }

        //흔들림 즉시 멈춤
        private void ResetShake()
        {
            if (noiseModule != null)
            {
                noiseModule.AmplitudeGain = 0f;
                noiseModule.FrequencyGain = 0f;
            }
        }

        //GameManager의 OnGameOver 이벤트 발생 시 호출
        private void HandleGameOver(DeathCause cause)
        {
            OnGameOver();
        }
        #endregion


    }
}
