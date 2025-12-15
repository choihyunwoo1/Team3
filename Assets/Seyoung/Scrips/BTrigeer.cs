using UnityEngine;

namespace Team3
{
    public class BTrigeer : MonoBehaviour
    {
        public Animator animator;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetTrigger("BTrigger");

                Destroy(gameObject);
            }
        }
    }
}