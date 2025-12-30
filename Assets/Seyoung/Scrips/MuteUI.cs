using UnityEngine;

namespace Team3
{
    public class MuteUI : MonoBehaviour
    {
        public GameObject mute;
        public GameObject unMute;

        public void Set(bool isMuted)
        {
            mute.SetActive(isMuted);
            unMute.SetActive(!isMuted);
        }
    }
}