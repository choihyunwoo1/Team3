using UnityEngine;

namespace Choi
{
    public class GroundCollider : MonoBehaviour
    {
        public Player player;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Ground"))
                player.SetGrounded(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ground"))
                player.SetGrounded(false);
        }
    }
}