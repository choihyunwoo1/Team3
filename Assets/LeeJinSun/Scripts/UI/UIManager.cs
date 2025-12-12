using UnityEngine;

namespace JS
{
    public class UIManager : MonoBehaviour
    {
        public GameObject readyUI;
        public GameObject pauseUI;
        public GameObject gameOverUI;

        private static UIManager instance;
        public static UIManager Instance => instance;

        private void Awake()
        {
            instance = this;
        }

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
    }
}