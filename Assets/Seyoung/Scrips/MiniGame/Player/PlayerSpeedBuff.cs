using System.Collections;
using UnityEngine;

namespace Team3
{
    public class PlayerSpeedBuff : MonoBehaviour
    {
        public float buffMultiplier = 1.5f; // 속도 배율
        public float buffDuration = 3f;     // 지속 시간

        private PlayerMove playerMove;
        private float originalSpeed;
        private Coroutine buffCoroutine;

        private void Awake()
        {
            playerMove = GetComponent<PlayerMove>();
            originalSpeed = playerMove.GetMoveSpeed();
        }

        public void ActivateSpeedBuff()
        {
            if (buffCoroutine != null)
                StopCoroutine(buffCoroutine);

            buffCoroutine = StartCoroutine(SpeedBuffRoutine());
        }

        private IEnumerator SpeedBuffRoutine()
        {
            playerMove.SetMoveSpeed(originalSpeed * buffMultiplier);

            yield return new WaitForSeconds(buffDuration);

            playerMove.SetMoveSpeed(originalSpeed);
            buffCoroutine = null;
        }
    }
}
