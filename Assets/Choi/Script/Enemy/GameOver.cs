using UnityEngine;

namespace Choi
{
    /// <summary>
    /// 이 콜라이더에 닿으면 당신은 죽습니다.
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        #region Variables
        private Collider2D collider;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();

            // 충돌한 오브젝트가 Player 컴포넌트를 가지고 있다면
            if (player != null)
            {
                // 장애물 처리 (GameOver 호출)
                player.GameOver();

                // 선택 사항: 플레이어가 죽었으므로 장애물 오브젝트를 파괴하거나 비활성화합니다.
                // Destroy(gameObject); 
                // 또는 gameObject.SetActive(false);
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }
}