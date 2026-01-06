using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team3
{
    public class GameOverUI : MonoBehaviour
    {
        public TMP_Text scoreText;
        public TMP_Text bestScoreText;
        public GameObject newText;

        private string loadToScene = "MainMenu";


        private void OnEnable()
        {
            int finalScore = ScoreManager.Instance.GetScore();
            scoreText.text = finalScore.ToString();

            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (finalScore > bestScore)
            {
                bestScore = finalScore;
                PlayerPrefs.SetInt("BestScore", bestScore);

                if (newText != null)
                    newText.SetActive(true);
            }
            bestScoreText.text = bestScore.ToString();
        }

        public void Retry()
        {
            string nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }

        public void Menu()
        {
            SceneManager.LoadScene(loadToScene);

        }
    }
}
