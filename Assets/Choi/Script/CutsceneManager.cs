using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Choi
{
    public class CutsceneManager : MonoBehaviour
    {
        public static CutsceneManager Instance;

        public UnityEvent<DeathCause> OnCutsceneFinished = new UnityEvent<DeathCause>();

        [System.Serializable]
        public class DeathCutsceneData
        {
            public DeathCause cause;
            public GameObject cutsceneObj;
            public float duration = 2.5f;
        }

        [SerializeField]
        private List<DeathCutsceneData> cutsceneList = new List<DeathCutsceneData>();

        private Dictionary<DeathCause, DeathCutsceneData> cutsceneDict;

        private bool isPlaying = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            cutsceneDict = new Dictionary<DeathCause, DeathCutsceneData>();
            foreach (var data in cutsceneList)
            {
                if (!cutsceneDict.ContainsKey(data.cause))
                    cutsceneDict.Add(data.cause, data);
            }
        }

        public void PlayDeathCutscene(DeathCause cause)
        {
            if (isPlaying)
                return;

            // 게임 매니저에게 먼저 죽음 알림
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
                gm.RequestGameOver(cause);

            if (cutsceneDict.TryGetValue(cause, out var data))
            {
                StartCoroutine(Play(data.cutsceneObj, data.duration, cause));
            }
        }

        private IEnumerator Play(GameObject obj, float duration, DeathCause cause)
        {
            isPlaying = true;
            obj.SetActive(true);

            // Animator 강제 실행
            Animator anim = obj.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.Rebind();     // 상태 초기화
                anim.Update(0f);   // 강제 반영
                anim.Play(0);      // 첫 스테이트 재생
            }

            yield return new WaitForSecondsRealtime(duration);

            obj.SetActive(false);
            isPlaying = false;

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
                gm.NotifyGameOverCutsceneFinished();

            OnCutsceneFinished?.Invoke(cause);
        }
    }
}
