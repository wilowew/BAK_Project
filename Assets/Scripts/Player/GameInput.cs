using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    private bool inputEnabled = true;

    public event EventHandler OnPlayerAttack;
    public event EventHandler<bool> OnInputStateChanged;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        if (inputEnabled && OnPlayerAttack != null) 
        {
            OnPlayerAttack.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        if (!inputEnabled) return Vector3.zero;
        return Mouse.current.position.ReadValue();
    }

    public void DisableMovement()
    {
        inputEnabled = false;
        OnInputStateChanged?.Invoke(this, inputEnabled);
    }

    public void EnableMovement()
    {
        inputEnabled = true;
        OnInputStateChanged?.Invoke(this, inputEnabled);
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    private void OnDestroy()
    {
        playerInputActions.Disable();
    }
}
