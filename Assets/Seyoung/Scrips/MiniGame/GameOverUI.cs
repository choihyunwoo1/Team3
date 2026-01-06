using Choi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    #region Variables

    //메뉴씬
    private string loadToScene = "MainMenu";
    private string ReadyUI = "Ready";

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI newText;

    #endregion

    #region Unity Event Method
    private void OnEnable()
    {
        //게임오버 UI 값 설정
        scoreText.text = MiniGameManager.Score.ToString();

        //베스트 스코어
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        //베스트 스코어와 현재 스코어 비교해서 베스트 스코어 갱신
        if (MiniGameManager.Score > bestScore)
        {
            bestScore = MiniGameManager.Score;
            //베스트 스코어 저장
            PlayerPrefs.SetInt("BestScore", bestScore);

            //UI
            newText.gameObject.SetActive(true);
        }
        bestScoreText.text = bestScore.ToString();
    }
    #endregion

    #region Custom Method
    public void Retry()
    {
        string nowScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nowScene);
    }

    public void Menu()
    {
        SceneManager.LoadScene(loadToScene);

    }

    #endregion
}
