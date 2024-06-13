using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string moveActionName = "Move";
        [SerializeField] private string runActionName = "Run";

        private void OnEnable()
        {
            var moveAction = inputActions.FindAction(moveActionName);
            if (moveAction != null)
            {
                moveAction.performed += HandleMoveInput;
                moveAction.canceled += HandleMoveInput;
            }
            var runAction = inputActions.FindAction(runActionName);
            if (runAction != null)
            {
                runAction.started += HandleRunInputStarted;
                runAction.canceled += HandleRunInputCanceled;
            }
            //TODO: Unsubcribe to these events - SF (self added)
        }

        private void HandleRunInputStarted(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic
            Debug.Log($"{name}: Run input started");
        }

        private void HandleRunInputCanceled(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic
            Debug.Log($"{name}: Run input canceled");
        }

        private void HandleMoveInput(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic
        }
    }
}