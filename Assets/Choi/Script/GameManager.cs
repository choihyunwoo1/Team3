using UnityEngine;

namespace Choi
{
    public enum GameState
    {
        Ready,   
        Playing,
        Paused,
        Cutscene,
        GameOver
    }

    /// <summary>
    /// 게임 전체의 흐름을 관리하는 클래스
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        //게임오버 체크
        static bool isStart;
        static bool isDeath;
        #endregion

        #region Property
        public static bool IsStart
        {
            get { return isStart; }
            set {isStart = value;}
        }
        public static bool IsDeath
        {
            get { return isDeath; }
            set { isDeath = value; }
        }

        //현재 상태
        public static GameState State { get; private set; }
        //컷인
        //public static bool IsCutscenePlaying { get; private set; }
        #endregion

        #region Unity Event Method
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            //초기화
            isStart = false;
            isDeath = false;
            //IsCutscenePlaying = false

        }
        void Update()
        {
            if (State == GameState.Ready)
            {
                if (Input.anyKeyDown)
                {
                    SetState(GameState.Playing);
                }
            }

            if (State == GameState.Playing)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SetState(GameState.Paused);
                }
            }
            else if (State == GameState.Paused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SetState(GameState.Playing);
                }
            }

#if UNITY_EDITOR //유니티 에디터 안에서만 사용가능

            //치팅 - 저장 데이터 삭제
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayerPrefs.DeleteAll();
            }
#endif
        }
        #endregion

        #region Custom Method
         public static void SetState(GameState newState)
         {
             State = newState;

            // 모든 UI를 초기 비활성화
            UIManager.Instance.HideReady();
            UIManager.Instance.HidePause();
            UIManager.Instance.HideGameOver();

            switch (newState)
            {
                case GameState.Ready:
                    Time.timeScale = 0f;
                    UIManager.Instance.ShowReady();
                    break;

                case GameState.Playing:
                    Time.timeScale = 1f;
                    UIManager.Instance.HideReady();
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    UIManager.Instance.ShowPause();
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    UIManager.Instance.ShowGameOver();
                    break;
            }
         }


        //컷씬중 PAUSE
        /* public IEnumerator PlayCutscene(System.Action action, float time)
         {
             IsCutscenePlaying = true;

             Time.timeScale = 0f; // 전체 정지

             action?.Invoke();    // 컷씬 실행(애니메이션, UI 등)

             yield return new WaitForSecondsRealtime(time);

             Time.timeScale = 1f; // 재개
             IsCutscenePlaying = false;
         }*/
        #endregion

    }
}