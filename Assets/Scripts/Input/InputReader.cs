using UnityEngine;
using UnityEngine.InputSystem;
using Events;
using Core;

namespace Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputReader : MonoBehaviour
    {
        [Header("References")]
        [Header("Inputs")]
        [SerializeField] private InputActionAsset inputActions;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private InputAction _moveAction;
        private InputAction _runAction;

        private void Awake()
        {
            if (!inputActions)
            {
                Debug.LogError($"{name}: {nameof(inputActions)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _moveAction = inputActions.FindAction(GameEvents.MoveAction);

            if (_moveAction != null)
            {
                _moveAction.performed += HandleMoveInput;
                _moveAction.canceled += HandleMoveInput;
            }

            _runAction = inputActions.FindAction(GameEvents.RunAction);

            if (_runAction != null)
            {
                _runAction.started += HandleRunInputStarted;
                _runAction.canceled += HandleRunInputCanceled;
            }
        }

        private void OnDisable()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= HandleMoveInput;
                _moveAction.canceled -= HandleMoveInput;
            }

            if (_runAction != null)
            {
                _runAction.started -= HandleRunInputStarted;
                _runAction.canceled -= HandleRunInputCanceled;
            }
        }

        private void HandleRunInputStarted(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE
            if (enableLogs) Debug.Log($"{name}: <color=magenta> Run input started </color>");

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.RunAction, true);
        }

        private void HandleRunInputCanceled(InputAction.CallbackContext ctx)
        {
            //TODO: Implement event logic | DONE
            if (enableLogs) Debug.Log($"{name}: <color=magenta> Run input canceled </color>");

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