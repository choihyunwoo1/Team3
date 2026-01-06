using Choi;
using UnityEngine;

namespace Team3
{
    public class SpawnManager : MonoBehaviour
    {
        #region Variables
        //기둥 프리팹 오브젝트
        public GameObject meteorPrefab;

        //스폰 타이머
        public float spawnTimer = 1f;  //타이머 기준 시간
        private float countdown = 0f;   //시간 누적 변수

        //스폰 높이 랜덤 범위 설정
        private float MinspawnX = -1.5f;
        private float MaxspawnX = 4.5f;
        #endregion

        #region Unity Event Method

        private void Start()
        {
            //초기화
            spawnTimer = 1f;
        }
        private void Update()
        {

            //1초에 하나씩 스폰
            countdown += Time.deltaTime;
            if (countdown >= spawnTimer)
            {
                //타이머 기능 실행
                MeteorSpawn();
                //타이머 초기화
                countdown = 0f;
                spawnTimer = 1f - MiniGameManager.spawnValue;
            }
        }

        #endregion

        #region Custom Method
        private void MeteorSpawn()
        {
            Debug.Log("SpawnTry");

            float spawnX = this.transform.position.x + Random.Range(MinspawnX, MaxspawnX);
            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);
            Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        }

        #endregion
    }
}