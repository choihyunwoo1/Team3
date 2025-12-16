using UnityEngine;

namespace Choi
{
    public class DTrigger : MonoBehaviour
    {
        private bool triggered = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered)
                return;

            Player player = other.GetComponent<Player>();
            if (player == null)
                return;

            player.ReverseDirection();
            triggered = true;

            // 필요하면 트리거 제거
            gameObject.SetActive(false);
        }
    }
}