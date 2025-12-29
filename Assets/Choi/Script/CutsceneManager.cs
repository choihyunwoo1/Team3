using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Choi
{
    public class CutsceneManager : MonoBehaviour
    {
        public static CutsceneManager Instance;

        // 기존 DeathCutscene 전용 이벤트
        public UnityEvent<DeathCause> OnCutsceneFinished = new UnityEvent<DeathCause>();

        [System.Serializable]
        public class DeathCutsceneData
        {
            public DeathCause cause;
            public GameObject cutsceneObj;
            public float duration = 2.5f;
        }

        // NEW ------------------------------
        [System.Serializable]
        public class FinishCutsceneData
        {
            public GameObject cutsceneObj;
            public float duration = 2.0f;
        }
        // ----------------------------------

        [SerializeField] private List<DeathCutsceneData> cutsceneList = new List<DeathCutsceneData>();
        private Dictionary<DeathCause, DeathCutsceneData> cutsceneDict;

        [Header("Finish Cutscene")]
        [SerializeField] private FinishCutsceneData finishCutscene;  // NEW

        private bool isPlaying = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // DeathCutscene 초기화
            cutsceneDict = new Dictionary<DeathCause, DeathCutsceneData>();
            foreach (var data in cutsceneList)
            {
                if (!cutsceneDict.ContainsKey(data.cause))
                    cutsceneDict.Add(data.cause, data);
            }
        }

        // -------------------------
        // 기존 Death 컷씬 재생
        // -------------------------
        public void PlayDeathCutscene(DeathCause cause)
        {
            if (isPlaying)
                return;

            GameManager gm = FindObjectOfType<GameManager>();
            gm?.RequestGameOver(cause);

            if (cutsceneDict.TryGetValue(cause, out var data))
            {
                StartCoroutine(PlayDeath(data.cutsceneObj, data.duration, cause));
            }
        }

        private IEnumerator PlayDeath(GameObject obj, float duration, DeathCause cause)
        {
            isPlaying = true;
            obj.SetActive(true);

            // Animator 강제 초기화
            Animator anim = obj.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.Rebind();
                anim.Update(0f);
                anim.Play(0);
            }

            yield return new WaitForSecondsRealtime(duration);

            obj.SetActive(false);
            isPlaying = false;

            GameManager gm = FindObjectOfType<GameManager>();
            gm?.NotifyGameOverCutsceneFinished();

            OnCutsceneFinished?.Invoke(cause);
        }

        // -------------------------
        // NEW: Finish 컷씬 재생
        // -------------------------
        public void PlayFinishCutscene()
        {
            if (isPlaying)
                return;

            StartCoroutine(PlayFinish());
        }

        private IEnumerator PlayFinish()
        {
            isPlaying = true;

            GameObject obj = finishCutscene.cutsceneObj;
            obj.SetActive(true);

            Animator anim = obj.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.Rebind();
                anim.Update(0f);
                anim.Play(0);
            }

            yield return new WaitForSecondsRealtime(finishCutscene.duration);

            obj.SetActive(false);
            isPlaying = false;

            // GameManager에 Finish 종료 알림
            GameManager gm = FindObjectOfType<GameManager>();
            gm?.NotifyFinishCutsceneFinished();
        }
    }
}
