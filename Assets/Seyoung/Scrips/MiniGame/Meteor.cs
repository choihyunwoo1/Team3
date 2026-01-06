using UnityEngine;

namespace Team3
{ 

    public class Meteor : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // PlayerMove 스크립트 가져오기
                PlayerMove playerMove = other.gameObject.GetComponent<PlayerMove>();
                if (playerMove != null)
                {
                    playerMove.Die(); // 안전하게 호출
                    
                }
                else
                {
                    Debug.LogWarning("PlayerMove 스크립트가 Player에 없음!");
                }

                // 운석 파괴
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                // 바닥에 닿으면 운석 파괴
                Destroy(gameObject);
            }
        }
    }
}
