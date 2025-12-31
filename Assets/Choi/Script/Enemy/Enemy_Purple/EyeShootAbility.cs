using UnityEngine;
using System.Collections;

namespace Choi
{
    public class EyeShootAbility : MonoBehaviour, IEnemyAbility
    {
        #region Variables
        private Enemy_Main owner;
        private GameManager gameManager;

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject eyeVisual;

        private bool initialized = false;

        private enum State
        {
            ChaseBehind,
            DashForward,
            MoveUp,
            ReturnBehind
        }

        private State currentState = State.ChaseBehind;

        [Header("Offsets")]
        [SerializeField] private float behindOffsetX = 8f;     // 플레이어 뒤 X
        [SerializeField] private float behindOffsetY = 3f;     // 플레이어보다 위

        [Header("Speeds")]
        [SerializeField] private float chaseSpeed = 6f;
        [SerializeField] private float dashSpeed = 18f;
        [SerializeField] private float upSpeed = 12f;
        [SerializeField] private float returnSpeed = 8f;

        [Header("Dash Settings")]
        [SerializeField] private float dashInterval = 3f;  // 일정 시간마다 Dash
        [SerializeField] private float upDistance = 4f;    // Dash 후 상승 높이

        private float dashTimer = 0f;
        private Vector3 upTarget;

        private int facingDir = 1;
        #endregion

        #region IEnemyAbility Methods
        public void Setup(Enemy_Main enemy)
        {
            owner = enemy;
            gameManager = FindAnyObjectByType<GameManager>();

            if (eyeVisual != null)
            {
                animator = eyeVisual.GetComponent<Animator>();
            }

            initialized = true;
        }

        public void OnEnter()
        {
            if (!initialized) return;

            currentState = State.ChaseBehind;
            dashTimer = 0f;

            if (eyeVisual != null)
                eyeVisual.SetActive(true);
        }

        public void OnExit()
        {
            if (eyeVisual != null)
                eyeVisual.SetActive(false);
        }

        public void OnTick()
        {
            if (owner.player == null) return;

            UpdateFacingDirection();

            switch (currentState)
            {
                case State.ChaseBehind:
                    Tick_ChaseBehind();
                    break;

                case State.DashForward:
                    Tick_DashForward();
                    break;

                case State.MoveUp:
                    Tick_MoveUp();
                    break;

                case State.ReturnBehind:
                    Tick_ReturnBehind();
                    break;
            }
        }
        #endregion

        #region State Behaviors
        private void Tick_ChaseBehind()
        {
            dashTimer += Time.deltaTime;

            Vector3 target = owner.player.position
                + new Vector3(behindOffsetX * facingDir, behindOffsetY, 0);

            MoveTowards(target, chaseSpeed);

            if (dashTimer >= dashInterval)
            {
                dashTimer = 0;
                currentState = State.DashForward;
            }
        }

        private void Tick_DashForward()
        {
            Vector3 dashTarget = owner.player.position
                + new Vector3(-2f * facingDir, 0, 0);

            MoveTowards(dashTarget, dashSpeed);

            if (Vector2.Distance(transform.position, dashTarget) < 0.3f)
            {
                upTarget = new Vector3(
                    transform.position.x,
                    transform.position.y + upDistance,
                    transform.position.z
                );

                currentState = State.MoveUp;

                if (animator != null)
                    animator.SetTrigger("IsJump");
            }
        }

        private void Tick_MoveUp()
        {
            MoveTowards(upTarget, upSpeed);

            if (Vector2.Distance(transform.position, upTarget) < 0.2f)
            {
                currentState = State.ReturnBehind;
            }
        }

        private void Tick_ReturnBehind()
        {
            Vector3 target = owner.player.position
                + new Vector3(behindOffsetX * facingDir, behindOffsetY, 0);

            MoveTowards(target, returnSpeed);

            if (Vector2.Distance(transform.position, target) < 0.3f)
            {
                currentState = State.ChaseBehind;
            }
        }
        #endregion

        #region Helpers
        private void MoveTowards(Vector3 target, float speed)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
        }

        private void UpdateFacingDirection()
        {
            facingDir = (owner.player.position.x > transform.position.x) ? 1 : -1;

            if (eyeVisual != null)
                eyeVisual.transform.localScale = new Vector3(facingDir, 1, 1);
        }
        #endregion
    }
}
