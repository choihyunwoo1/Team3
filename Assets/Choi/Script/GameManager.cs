using System;
using UnityEngine;

namespace Choi
{
    public class GameManager : MonoBehaviour, IGameStateProvider
    {
        #region Variables
        public GameState State { get; private set; } = GameState.Ready;
        public GameState CurrentState => State;

        public event Action<GameState> OnStateChanged;
        public event Action<DeathCause> OnGameOver;
        [SerializeField] private Enemy_Main enemy;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            SetState(GameState.Ready);
        }

        private void Update()
        {
            // GameOver / Cutscene / StageClearCutscene 중 입력 금지
            if (State == GameState.GameOver ||
                State == GameState.GameOverCutscene ||
                State == GameState.StageClearCutscene)
                return;

            switch (State)
            {
                case GameState.Ready:
                    if (Input.anyKeyDown)
                        SetState(GameState.Playing);
                    break;

                case GameState.Playing:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        SetState(GameState.Paused);
                    break;

                case GameState.Paused:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        SetState(GameState.Playing);
                    break;
            }
        }
        #endregion

        #region Custom Method
        public void SetState(GameState newState)
        {
            if (State == newState)
                return;

            State = newState;

            // 플레이 중만 timeScale = 1
            // 나머지는 모두 멈춤
            Time.timeScale = newState == GameState.Playing ? 1f : 0f;

            OnStateChanged?.Invoke(State);
        }

        // Death (기존)
        public void RequestGameOver(DeathCause cause)
        {
            if (State == GameState.GameOver)
                return;

            SetState(GameState.GameOverCutscene);
            OnGameOver?.Invoke(cause);
        }

        public void NotifyGameOverCutsceneFinished()
        {
            if (State != GameState.GameOverCutscene)
                return;

            SetState(GameState.GameOver);
        }

        // NEW --------------------------
        // Stage Clear 요청 시
        public void RequestStageClear()
        {
            if (State == GameState.StageClear ||
                State == GameState.StageClearCutscene)
                return;

            SetState(GameState.StageClearCutscene);
        }

        // NEW: FinishCutscene 종료 알림
        public void NotifyFinishCutsceneFinished()
        {
            if (State != GameState.StageClearCutscene)
                return;

            SetState(GameState.StageClear);
        }
        // --------------------------------

        public void BuffEnemy(EnemyBuffType type, float value)
        {
            enemy.ApplyBuff(type, value);
        }
        #endregion
    }
}
