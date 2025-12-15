using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public Animator animator;
    public GameObject triggerObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 애니메이션 실행
        if (animator != null)
        {
            animator.SetTrigger("OnTrigger");
        }

        // 오브젝트 활성화
        if (triggerObject != null)
        {
            triggerObject.SetActive(true);
        }

        // 마지막에 한 번만 제거
        Destroy(gameObject);
    }

}
