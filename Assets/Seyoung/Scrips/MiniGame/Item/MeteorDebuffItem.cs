using UnityEngine;
using System.Collections;

namespace Team3
{
    public class MeteorDebuffItem : MonoBehaviour
    {
        public float speedMultiplier = 0.5f; // 속도 50%
        public float scaleMultiplier = 0.6f; // 크기 60%
        [SerializeField]
        private bool isActive;
        public GameObject buffTxt;
        private Collider2D col;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            if (buffTxt != null)
                buffTxt.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            SpawnManager spawnManager = FindAnyObjectByType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.meteorSpeed *= speedMultiplier;
                spawnManager.meteorScale *= scaleMultiplier;
            }

            StartCoroutine(Cooldown());
        }
        private IEnumerator Cooldown()
        {
            isActive = true;
            col.enabled = false;
            if (buffTxt != null)
                buffTxt.SetActive(true);

            yield return new WaitForSeconds(10f);

            col.enabled = true;
            isActive = false;
            if (buffTxt != null)
                buffTxt.SetActive(false);

        }
    }
}
