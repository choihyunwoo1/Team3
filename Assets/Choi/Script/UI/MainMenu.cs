using UnityEngine;
using Team3;

namespace Choi
{
    /// <summary>
    /// 메인메뉴 씬을 관리하는 클래스
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        public string loadToScene = "PlayScene";

        public GameObject diaryUI;
        public GameObject optionUI;
        public MuteUI global;
        public MuteUI effect;
        public MuteUI bg;
        public ResetUI resetUI;
        public VibrateUI vibrateUI;
        #endregion

        #region Unity Event Method

        #endregion

        #region Custom Method
        public void OnPlayButton()
        {
            SceneFader.FadeTo(loadToScene);
        }

        public void OpenDiary()
        {
            diaryUI.SetActive(true);
        }
        public void CloseDiary()
        {
            diaryUI.SetActive(false);
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }

        public void OnOpitonButton()
        {
            optionUI.SetActive(true);
        }
        public void CloseOpitonButton()
        {
            optionUI.SetActive(false);
        }
        public void SetGlobalMute()
        {
            global.Set(true);
        }

        public void SetGlobalUnMute()
        {
            global.Set(false);
        }

        public void SetBGMute()
        {
            bg.Set(true);
        }

        public void SetBGUnMute()
        {
            bg.Set(false);
        }

        public void SetEffectMute()
        {
            effect.Set(true);
        }

        public void SetEffectUnMute()
        {
            effect.Set(false);
        }
        public void OnReset()
        {
            resetUI.Set(false);
        }
        public void CloseReset()
        {
            resetUI.Set(true);
        }
        public void OnVibrate()
        {
            vibrateUI.Set(true);
        }
        public void OffVibrate()
        {
            vibrateUI.Set(false);
        }
        #endregion
    }
}