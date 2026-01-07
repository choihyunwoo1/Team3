using UnityEngine;

namespace Team3
{
    public class PauseUI : MonoBehaviour
    {
        public GameObject pauseUI;

        private void Start()
        {
            pauseUI.SetActive(false);
        }
        public void Pause()
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f; // ⭐ 게임 정지

        }
    }
}