using UnityEngine;

namespace Choi
{
    public class MaskUIManager : MonoBehaviour
    {
        public static MaskUIManager Instance;

        [Header("Mask UI Objects")]
        [SerializeField] private GameObject maskA;
        [SerializeField] private GameObject maskB;
        [SerializeField] private GameObject maskC;
        [SerializeField] private GameObject maskD;
        [SerializeField] private GameObject maskE;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void UpdateUI()
        {
            if (ItemManager.Instance == null)
                return;

            maskA.SetActive(ItemManager.Instance.hasA);
            maskB.SetActive(ItemManager.Instance.hasB);
            maskC.SetActive(ItemManager.Instance.hasC);
            maskD.SetActive(ItemManager.Instance.hasD);
            maskE.SetActive(ItemManager.Instance.hasE);
        }
        public void HideAll()
        {
            maskA.SetActive(false);
            maskB.SetActive(false);
            maskC.SetActive(false);
            maskD.SetActive(false);
            maskE.SetActive(false);
        }
    }
}
