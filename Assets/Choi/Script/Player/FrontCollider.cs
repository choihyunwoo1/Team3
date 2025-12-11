using UnityEngine;

namespace Choi
{
    public class FrontCollider : MonoBehaviour
    {
        public Player player;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
                player.SetFrontBlocked(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Wall"))
                player.SetFrontBlocked(false);
        }
    }
}