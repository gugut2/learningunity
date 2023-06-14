using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;

    PlayerInput input = null;

    private void OnEnable() 
    {
        input = new PlayerInput();
        input.Gameplay.Enable();

        input.Gameplay.Move.performed += SetMove;
        input.Gameplay.Move.canceled += SetMove;
    }

    private void OnDisable() 
    {
        input.Gameplay.Move.performed -= SetMove;
        input.Gameplay.Move.canceled -= SetMove;

        input.Gameplay.Disable();
    }

    private void SetMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
