using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Util.CustomEnum;

public class InputController : MonoBehaviour, InputActions.IPlayerActions
{
    private InputActions inputActions;
    private InputActionMap playerActionMap;
    private Action<InputAction.CallbackContext> onMove;
    private Action<InputAction.CallbackContext> onInteract;

    public void InitSettings()
    {
        inputActions = new InputActions();
        playerActionMap = inputActions.Player;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Interact.performed += OnInteract;
    }

    public void BindPlayerInputAction(PlayerAction playerAction, Action<InputAction.CallbackContext> someEvent)
    {
        switch (playerAction)
        {
            case PlayerAction.Move:
                onMove -= someEvent;
                onMove += someEvent;
                break;
                case PlayerAction.Interact:
                onInteract -= someEvent;
                onInteract += someEvent;
                break;
            default:
                break;
        }
    }

    public InputAction GetAction(PlayerAction playerAction)
    {
        switch (playerAction)
        {
            case PlayerAction.Move:
                return inputActions.Player.Move;
            case PlayerAction.Interact:
                return inputActions.Player.Interact;
            default:
                return null;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        onMove?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        onInteract?.Invoke(context);
    }

    public void OnEnable()
    {
        inputActions.Player.Move.Enable();
        inputActions.Player.Interact.Enable();
    }

    public void OnDisable()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.Interact.Disable();
    }
}