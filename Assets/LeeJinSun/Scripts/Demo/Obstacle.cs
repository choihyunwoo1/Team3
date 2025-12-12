using UnityEngine;

namespace JS
{
    public class Obstacle : MonoBehaviour
    {

        
            private void OnCollisionEnter2D(Collision2D collision)
            {
                // Player 태그와 충돌했을 때만 처리
                if (collision.collider.CompareTag("Player"))
                {
                    Player player = collision.collider.GetComponent<Player>();
                    if (player != null)
                    {
                        player.GameOver();
                    }
                }
            }
    }
}