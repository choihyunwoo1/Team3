using TMPro;
using UnityEngine;

namespace Team3
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        [SerializeField] private TMP_Text scoreText;

        private int totalScore = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddScore(int value)
        {
            totalScore += value;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (scoreText != null)
                scoreText.text = totalScore.ToString();
        }

        public int GetScore()
        {
            return totalScore;
        }
    }
}
