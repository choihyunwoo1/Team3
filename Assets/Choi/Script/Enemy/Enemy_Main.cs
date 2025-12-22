using UnityEngine;
using System.Collections.Generic;

namespace Choi
{
    public enum EnemyBuffType
    {
        None,
        SpeedUp,
        ScaleUp,
        LaserBeam,
        Red,
        Blue,
        Green,
    }

    public enum EnemyMoveState
    {
        Chasing,
        MovingToWaypoint
    }

    public class Enemy_Main : MonoBehaviour
    {
        #region Variables - 공통 설정
        [Header("References")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject defaultVisual;
        public Transform player;

        [Header("Movement Settings")]
        [SerializeField] private float speed = 3f;
        [SerializeField] private float maxScale = 4f;
        [SerializeField] private float floatAmplitude = 0.3f;
        [SerializeField] private float floatFrequency = 3f;

        private float baseSpeed;
        private Vector3 baseScale;
        private EnemyMoveState moveState = EnemyMoveState.Chasing;
        private Transform waypointTarget;
        #endregion

        #region Ability Management
        private IEnemyAbility currentAbility;
        private EnemyBuffType currentBuff = EnemyBuffType.None;

        private Dictionary<EnemyBuffType, IEnemyAbility> abilityMap = new Dictionary<EnemyBuffType, IEnemyAbility>();
        #endregion

        #region Unity Event Methods
        private void Awake()
        {
            baseSpeed = speed;
            baseScale = transform.localScale;

            // IEnemyAbility 연결
            IEnemyAbility[] abilities = GetComponents<IEnemyAbility>();
            foreach (var ability in abilities)
            {
                ability.Setup(this);

                if (ability is PunchAbility) abilityMap[EnemyBuffType.Red] = ability;
                else if (ability is SlimeAbility) abilityMap[EnemyBuffType.Blue] = ability;
                else if (ability is LaughAbility) abilityMap[EnemyBuffType.Green] = ability;
                // if (ability is LaserAbility) abilityMap[EnemyBuffType.LaserBeam] = ability;
            }
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Update()
        {
            if (gameManager.State != GameState.Playing) return;

            HandleBaseMovement();

            // 현재 능력 갱신
            currentAbility?.OnTick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Enemy가 Player를 잡으면 사망 처리
            Player playerComponent = other.GetComponent<Player>();

            if (playerComponent != null)
            {
                playerComponent.Die(DeathCause.EnemyA);
                gameObject.SetActive(false);
            }
        }
        #endregion

        #region Public Methods - 버프 적용
        public void ApplyBuff(EnemyBuffType type, float value)
        {
            if (currentBuff == type) return;

            // 이전 능력 제거
            if (currentAbility != null)
            {
                currentAbility.OnExit();
            }
            else
            {
                defaultVisual.SetActive(false);
            }

            HandleStatBuffs(type, value);

            // 능력 연결
            if (abilityMap.TryGetValue(type, out IEnemyAbility newAbility))
            {
                currentAbility = newAbility;
                currentBuff = type;
                currentAbility.OnEnter();
            }
            else
            {
                currentAbility = null;
                currentBuff = EnemyBuffType.None;
                defaultVisual.SetActive(true);
            }
        }

        public void HandleStatBuffs(EnemyBuffType type, float value)
        {
            speed = baseSpeed;
            transform.localScale = baseScale;

            if (type == EnemyBuffType.SpeedUp)
                speed *= value;

            if (type == EnemyBuffType.ScaleUp)
            {
                Vector3 newScale = baseScale * value;

                if (newScale.x > maxScale)
                    newScale = Vector3.one * maxScale;

                transform.localScale = newScale;
            }
        }
        #endregion

        #region 이동 로직 통합
        private void HandleBaseMovement()
        {
            switch (moveState)
            {
                case EnemyMoveState.MovingToWaypoint:
                    MoveToWaypoint();
                    break;
                case EnemyMoveState.Chasing:
                    FollowPlayerGhostStyle();
                    CatchUpIfTooFar();
                    break;
            }
        }

        private void FollowPlayerGhostStyle()
        {
            if (player == null) return;

            float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

            float yTarget = Mathf.Lerp(
                transform.position.y,
                player.position.y,
                0.15f
            ) + offsetY;

            float xTarget = Mathf.Lerp(
                transform.position.x,
                player.position.x,
                speed * Time.deltaTime
            );

            Vector3 target = new Vector3(xTarget, yTarget, transform.position.z);

            transform.position = Vector3.Lerp(
                transform.position,
                target,
                speed * Time.deltaTime
            );
        }

        private void MoveToWaypoint()
        {
            if (waypointTarget == null)
            {
                moveState = EnemyMoveState.Chasing;
                return;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(
                transform.position,
                waypointTarget.position,
                step
            );

            float distance = Vector3.Distance(transform.position, waypointTarget.position);
            if (distance < 0.1f)
            {
                waypointTarget = null;
                moveState = EnemyMoveState.Chasing;
            }
        }

        private void CatchUpIfTooFar()
        {
            if (player == null) return;

            if (player.position.x - transform.position.x > 10f)
            {
                transform.position = new Vector3(
                    player.position.x - 8f,
                    transform.position.y,
                    transform.position.z
                );
            }
        }

        public void GoToWaypoint(Transform waypoint)
        {
            if (waypoint == null) return;
            waypointTarget = waypoint;
            moveState = EnemyMoveState.MovingToWaypoint;
        }
        #endregion
    }
}
