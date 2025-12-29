using UnityEngine;

namespace Choi
{
    public class FinishTrigger : MonoBehaviour
    {
        private bool triggered = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered)
                return;

            Player player = other.GetComponent<Player>();
            if (player == null)
                return;

            triggered = true;

            // 1) GameManager에 StageClear 요청
            GameManager gm = FindObjectOfType<GameManager>();
            gm?.RequestStageClear();

            // 2) Cutscene 재생
            CutsceneManager.Instance.PlayFinishCutscene();

            gameObject.SetActive(false);
        }
    }
}
