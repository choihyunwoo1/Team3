using UnityEngine;

namespace Team3
{
    public class MiniGameEnemy : MonoBehaviour
    {
        // 데미지 1당 점수 배율
        public int scorePerDamage = 1;

        public void TakeDamage(int damage)
        {
            if (ScoreManager.Instance != null)
            {
                int score = damage * scorePerDamage;
                ScoreManager.Instance.AddScore(score);
            }
        }
    }
}
