using UnityEngine;

public interface TriggerController 
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("OnTrigger");
        }
    }
}
