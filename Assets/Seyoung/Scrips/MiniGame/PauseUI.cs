using UnityEngine;

namespace Team3
{
    public class PauseUI : MonoBehaviour
    {
        public GameObject pauseUI;

        private bool pause = false;

        private void Start()
        {
            pauseUI.SetActive(false);
        }
        public void Pause()
        {
            pauseUI.SetActive(true);
            pause = true;
        }
    }
}