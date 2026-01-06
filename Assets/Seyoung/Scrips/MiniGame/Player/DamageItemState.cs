using UnityEngine;

namespace Team3
{
    public class DamageItemState : MonoBehaviour
    {
        public bool HasItem { get; private set; }

        public void Activate()
        {
            HasItem = true;
        }

        public void Consume()
        {
            HasItem = false;
        }
    }
}
