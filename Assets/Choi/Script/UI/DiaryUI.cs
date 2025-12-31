using System.Collections.Generic;
using UnityEngine;

namespace Choi
{
    public class DiaryUI : MonoBehaviour
    {
        [System.Serializable]
        public class DiaryButton
        {
            public string cutsceneKey;   
            public GameObject buttonObj;
        }

        [SerializeField]
        private List<DiaryButton> diaryButtons;

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            foreach (var item in diaryButtons)
            {
                bool unlocked = DiarySystem.Instance.IsUnlocked(item.cutsceneKey);
                item.buttonObj.SetActive(unlocked);
            }
        }
    }
}
