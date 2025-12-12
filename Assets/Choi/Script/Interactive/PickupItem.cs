using UnityEngine;

namespace Choi
{
    /// <summary>
    /// 이 클래스를 부모로 상속시켜 아이템 구현 가능
    /// </summary>
    public class PickupItem : MonoBehaviour
    {
        #region Variables

        #endregion

        #region Unity Event Method
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //충돌한 오브젝트에서 Player 컴포넌트를 찾기
            Player player = collision.GetComponent<Player>();

            if (player == null)
            {
                return;
            }

            //PickUp 메서드에 Player 인스턴스를 전달합니다.
            bool isPickupSuccessful = PickUp(player);

            if (isPickupSuccessful)
            {
                //성공적으로 사용했을 경우에만 파괴
                Destroy(gameObject);
            }
        }
        #endregion

        #region Custom Method
        // 픽업 시 아이템 효과 구현 - 성공시 true, 실패시 false
        //인수를 Player로 변경
        protected virtual bool PickUp(Player player)
        {
            // 자식 클래스에서 구현할 로직 (기본값: 미사용)
            return false;
        }
        #endregion
    }
}