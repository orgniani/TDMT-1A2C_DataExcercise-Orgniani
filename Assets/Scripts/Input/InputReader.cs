using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string attackActionName = "Attack";

    private void OnEnable()
    {
        inputActions.FindAction(moveActionName).performed += HandleMoveInput;
        inputActions.FindAction(attackActionName).started += HandleAttackInput;
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        //TODO: Implement event logic
        Debug.Log($"{name}: Attack input pressed");
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        //TODO: Implement event logic
        Debug.Log($"{name}: Move input performed");
    }
}
