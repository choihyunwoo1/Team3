using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    /// <summary>
    /// 슬라임 UI 흘러내리면서 사라지는 효과
    /// </summary>
    public class SlimeEffect : MonoBehaviour
    {
        #region Variables
        public Image img;
        private RectTransform rectTransform;

        [Header("Settings")]
        public float fadeSpeed = 0.5f;   // 투명해지는 속도
        public float fallSpeed = 50f;    // 아래로 내려가는 속도 (픽셀 단위)
        #endregion

        #region Unity Event Method
        void Awake()
        {
            img = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        // 오브젝트가 활성화될 때마다 초기 알파값 세팅 (재사용 대비)
        void OnEnable()
        {
            if (img != null)
            {
                Color c = img.color;
                c.a = 1f;
                img.color = c;
            }
        }
        void Update()
        {
            if (img.color.a > 0)
            {
                // 1. 알파값 감소 (투명화)
                Color c = img.color;
                c.a -= fadeSpeed * Time.deltaTime;
                img.color = c;

                // 2. 위치 이동 (내려가기)
                // anchoredPosition을 사용하여 UI 좌표계에서 아래(-Y)로 이동
                rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
            }
            else
            {
                // 완전히 투명해지면 오브젝트 비활성화
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
