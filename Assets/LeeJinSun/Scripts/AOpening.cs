using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JS
{
    /// <summary>
    /// 플레이씬01의 오프닝 연출
    /// </summary>
    public class AOpening : MonoBehaviour
    {
        #region Variables
        //페이더 효과
        public SceneFader fader;

        //플레이어
        //public GameObject player;

        //시퀀스 텍스트
        //public TextMeshProUGUI sequenceText;

        //시나리오 텍스트
        //[SerializeField]
        //private string sequence01 = "...Where am I?";

        //[SerializeField]
       // private string sequence02 = "I need to get out of here";

        //사운드
        //public AudioSource line01;
        //public AudioSource line02;

        #endregion

        #region Unity Event Method
        void Start ()
        {

            //시작하자마자 오프닝 연출
            StartCoroutine(SequencePlay());
        }

        #endregion

        #region Custom Method
        //오프닝 시퀀스 연출
        IEnumerator SequencePlay()
        {  
            //캐릭터 비활성화
            //player.SetActive(false);

            //페이드인 연출 (1초 후 페이드인 효과) - 2초
            fader.FadeStart(2f+3f);

            //시나리오 텍스트 화면, 사운드 출력
            //sequenceText.text = sequence01;
            //line01.Play();

            yield return new WaitForSeconds(3f);

            //시나리오 텍스트 화면, 사운드 출력
            //sequenceText.text = sequence02;
            //line02.Play();

            //3초 후 텍스트 지움
            //yield return new WaitForSeconds(3f);
           // sequenceText.text = "";

            //플레이 캐릭터 활성화
           // player.SetActive(true);

        }

        #endregion
    }
}
