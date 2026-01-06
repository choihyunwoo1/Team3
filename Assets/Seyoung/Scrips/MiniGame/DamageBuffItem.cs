using System.Collections;
using TMPro;
using UnityEngine;

namespace Team3
{
    public class DamageBuffItem : MonoBehaviour
    {
        [SerializeField] private float cooldown = 10f;
        private Collider2D col;
        [SerializeField]
        private bool isActive;
        public GameObject buffTxt;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            if (buffTxt != null)
                buffTxt.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive) return;
            if (!other.CompareTag("Player")) return;

            DamageBuff buff = other.GetComponent<DamageBuff>();
            if (buff == null) return;

            buff.Activate();
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            isActive = true;
            col.enabled = false;
            if (buffTxt != null)
                buffTxt.SetActive(true);

            yield return new WaitForSeconds(cooldown);

            col.enabled = true;
            isActive = false;
            if (buffTxt != null)
                buffTxt.SetActive(false);

        }
    }
}
