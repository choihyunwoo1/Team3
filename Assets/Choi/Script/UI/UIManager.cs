using UnityEngine;

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
        #endregion
    }
}