using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
