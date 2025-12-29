using UnityEngine;
using System.Collections;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    #region Variables
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI tx;

    [Header("Settings")]
    [SerializeField] private string _text = "END?";       // 출력할 문장

    [Tooltip("글자 사이의 속도 (높을수록 천천히)")]
    [SerializeField] private float typingSpeed = 0.3f;

    // 상태를 저장하는 변수
    private bool isTypingFinished = false;
    #endregion

    #region Unity Event Method
    private void OnEnable()
    {
        if (tx == null) tx = GetComponent<TextMeshProUGUI>();

        // 1. 이미 타이핑이 끝난 상태라면?
        if (isTypingFinished)
        {
            // 다시 타이핑하지 않고 최종 문구만 보여주고 유지합니다.
            tx.text = _text;
            return;
        }

        // 2. 처음 실행되는 경우라면 타이핑 시작
        StopAllCoroutines();
        StartCoroutine(Typing());
    }
    #endregion

    #region Custom Method
    IEnumerator Typing()
    {
        tx.text = "";
        for (int i = 0; i <= _text.Length; i++)
        {
            tx.text = _text.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingFinished = true;
    }
    #endregion
}



