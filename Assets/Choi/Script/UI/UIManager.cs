using UnityEngine;
using UnityEngine.SceneManagement;

namespace Choi
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public GameObject readyUI;
        public GameObject pauseUI;
        public GameObject gameOverUI;

        private static UIManager instance;
        public static UIManager Instance => instance;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            instance = this;

            GameManager.SetState(GameState.Ready);
        }
        #endregion

        #region Custom Method
        public void ShowReady()
        {
            readyUI.SetActive(true);
        }
        public void HideReady()
        {
            readyUI.SetActive(false);
        }

        public void ShowPause()
        {
            pauseUI.SetActive(true);
        }
        public void HidePause()
        {
            pauseUI.SetActive(false);
        }

        public void ShowGameOver()
        {
            gameOverUI.SetActive(true);
        }
        public void HideGameOver()
        {
            gameOverUI.SetActive(false);
        }
        public void GoToMainMenu()
        {
            GameManager.IsDeath = false;
            GameManager.SetState(GameState.Ready);

            SceneFader.Instance.FadeTo("MainMenu");
        }
        public void Retry()
        {
            GameManager.IsDeath = false;
            GameManager.SetState(GameState.Playing);

            string sceneName = SceneManager.GetActiveScene().name;
            SceneFader.Instance.FadeTo(sceneName);
        }
        public void Continue()
        {
            // Pause UI 끄기
            HidePause();

            // 게임 다시 진행
            GameManager.SetState(GameState.Playing);

            GameManager.IsDeath = false;  // 혹시 죽은 상태였다면 클리어
        }
        #endregion
    }
}