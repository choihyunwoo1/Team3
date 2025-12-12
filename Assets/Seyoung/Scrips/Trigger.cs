using UnityEngine;

namespace My2DGame
{
    public class Trigger : MonoBehaviour
    {
        #region Variables
        public GameObject moveObject;
        #endregion

        #region Unity Event Method
        public void OnTriggerEnter2D(Collider2D other)
        {
            moveObject.SetActive(true);

            Destroy(this.gameObject);
        }
        #endregion

        #region Custom Method
        #endregion
    }
}