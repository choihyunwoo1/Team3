using UnityEngine;

namespace JS
{
    /// <summary>
    /// 펀치 공격하는 클래스
    /// </summary>
    public class Punch : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;

        //하강 속도
        [SerializeField]
        private Vector2 fallSpeed = new Vector2(0f, 5f);
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb2D.linearVelocity = new Vector2(fallSpeed.x, fallSpeed.y * transform.localScale.y);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die(DeathCause.EnemyA);
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }
}
