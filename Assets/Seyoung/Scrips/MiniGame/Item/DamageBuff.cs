using UnityEngine;

namespace Team3
{
    public class DamageBuff : MonoBehaviour
    {
        public bool HasBuff { get; private set; }

        public void Activate()
        {
            HasBuff = true;
        }

        public void Consume()
        {
            HasBuff = false;
        }
    }
}
