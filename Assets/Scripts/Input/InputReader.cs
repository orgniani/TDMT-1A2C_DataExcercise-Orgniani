using UnityEngine;
using UnityEngine.InputSystem;
using Events;
using Core;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private InputAction moveAction;
        private InputAction runAction;

        private void OnEnable()
        {
            moveAction = inputActions.FindAction(GameEvents.MoveAction);

            if (moveAction != null)
            {
                moveAction.performed += HandleMoveInput;
                moveAction.canceled += HandleMoveInput;
            }

            runAction = inputActions.FindAction(GameEvents.RunAction);

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
                EventManager<string>.Instance.InvokeEvent(GameEvents.RunAction, true);
        }

        private void HandleRunInputCanceled(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE
            Debug.Log($"{name}: Run input canceled");

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.RunAction, false);
        }

        private void HandleMoveInput(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE

            Vector3 inputDirection = ctx.ReadValue<Vector2>();

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.MoveAction, inputDirection);
        }
    }
}