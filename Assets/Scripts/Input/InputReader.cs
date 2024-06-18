using UnityEngine;
using UnityEngine.InputSystem;
using Events;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string moveActionName = "Move";
        [SerializeField] private string runActionName = "Run";

        private InputAction moveAction;
        private InputAction runAction;

        private void OnEnable()
        {
            moveAction = inputActions.FindAction(moveActionName);

            if (moveAction != null)
            {
                moveAction.performed += HandleMoveInput;
                moveAction.canceled += HandleMoveInput;
            }

            runAction = inputActions.FindAction(runActionName);

            if (runAction != null)
            {
                runAction.started += HandleRunInputStarted;
                runAction.canceled += HandleRunInputCanceled;
            }

            //TODO: Unsubscribe from events -SF | DONE
        }

        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.performed -= HandleMoveInput;
                moveAction.canceled -= HandleMoveInput;
            }

            if (runAction != null)
            {
                runAction.started -= HandleRunInputStarted;
                runAction.canceled -= HandleRunInputCanceled;
            }
        }

        private void HandleRunInputStarted(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE
            Debug.Log($"{name}: Run input started");

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(runActionName, true);
        }

        private void HandleRunInputCanceled(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE
            Debug.Log($"{name}: Run input canceled");

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(runActionName, false);
        }

        private void HandleMoveInput(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE

            Vector3 inputDirection = ctx.ReadValue<Vector2>();

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(moveActionName, inputDirection);
        }
    }
}