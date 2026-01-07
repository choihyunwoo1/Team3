using Choi;
using Team3;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject meteorPrefab;

    public float spawnTimer = 1f;
    private float countdown = 0f;

    private float MinspawnX = -20f;
    private float MaxspawnX = 20f;

    public PlayerMove player;

    // ⭐ 메테오 기본 옵션
    [Header("Meteor Option")]
    public float meteorSpeed = 5f;
    public float meteorScale = 1f;

    private void Update()
    {
        if (player.IsDead())
            return;

        countdown += Time.deltaTime;
        if (countdown >= spawnTimer)
        {
            MeteorSpawn();
            countdown = 0f;
            spawnTimer = 1f - MiniGameManager.spawnValue;
        }
    }

    private void MeteorSpawn()
    {
        float spawnX = transform.position.x + Random.Range(MinspawnX, MaxspawnX);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, transform.position.z);

        GameObject meteorObj = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
        Meteor meteor = meteorObj.GetComponent<Meteor>();

        if (meteor != null)
        {
            meteor.Init(meteorSpeed, meteorScale);
        }
    }
}
