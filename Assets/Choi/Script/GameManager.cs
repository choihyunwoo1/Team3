using UnityEngine;

namespace Choi
{
    /// <summary>
    /// 게임 전체의 흐름을 관리하는 클래스
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        static bool isStart;

        //게임오버 체크
        static bool isDeath;

        static int score;

        public static float spawnValue = 0f;
        #endregion

        #region Property
        public static bool IsStart
        {
            get { return isStart; }
            set {isStart = value;}
        }
        public static bool IsDeath
        {
            get { return isDeath; }
            set { isDeath = value; }
        }

        public static int Score
        {
            get { return score; }
            set { score = value; }  
        }

        #endregion

        #region Unity Event Method
        void Start()
        {
            //초기화
            isStart = false;
            isDeath = false;    
            score = 0;
            spawnValue = 0f;    
        }
        void Update()
        {
#if UNITY_EDITOR //유니티 에디터 안에서만 사용가능

            //치팅 - 저장 데이터 삭제
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayerPrefs.DeleteAll();
            }
#endif
        }
        #endregion

        #region Custom Method
       
        #endregion

    }
}