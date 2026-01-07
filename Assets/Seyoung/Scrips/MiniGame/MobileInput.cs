using UnityEngine;
using UnityEngine.EventSystems;

namespace Team3
{
    public class MobileInput : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler
    {
        public enum InputType { Left, Right, Jump }
        public InputType inputType;

        private static float horizontal;
        private static bool jump;

        public static float Horizontal => horizontal;
        public static bool Jump => jump;

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (inputType)
            {
                case InputType.Left:
                    horizontal = -1f;
                    break;

                case InputType.Right:
                    horizontal = 1f;
                    break;

                case InputType.Jump:
                    jump = true;
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (inputType == InputType.Left || inputType == InputType.Right)
                horizontal = 0f;

            if (inputType == InputType.Jump)
                jump = false;
        }
    }
}
