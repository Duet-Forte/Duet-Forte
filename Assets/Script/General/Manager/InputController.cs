using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Util.CustomEnum;
using System.Collections.Generic;

public class InputController : MonoBehaviour, InputActions.IPlayerActions
{
    private InputActions inputActions;
    private Dictionary<PlayerAction, InputAction> actionPairs;
    private Dictionary<PlayerAction, Action<InputAction.CallbackContext>> eventPairs;
    private Dictionary<PlayerAction, Action<InputAction.CallbackContext>> delegatePairs;
    private bool isInitialized = false;

    public void InitSettings()
    {
        inputActions = new InputActions();
        actionPairs = new Dictionary<PlayerAction, InputAction>()
        { 
            { PlayerAction.Move, inputActions.Player.Move },
            { PlayerAction.Interact, inputActions.Player.Interact },
            { PlayerAction.Skip, inputActions.Player.Skip }
        };
        eventPairs = new Dictionary<PlayerAction, Action<InputAction.CallbackContext>>()
        {
            { PlayerAction.Move, OnMove },
            { PlayerAction.Interact, OnInteract },
            { PlayerAction.Skip, OnSkip }
        };
        delegatePairs = new Dictionary<PlayerAction, Action<InputAction.CallbackContext>>()
        { 
            { PlayerAction.Move, null },
            { PlayerAction.Interact, null },
            { PlayerAction.Skip, null }
        };

        foreach(var actionPair in actionPairs)
        {
            actionPair.Value.performed -= eventPairs[actionPair.Key];
            actionPair.Value.performed += eventPairs[actionPair.Key];
        }
        isInitialized = true;
    }

    public void BindPlayerInputAction(PlayerAction playerAction, Action<InputAction.CallbackContext> someEvent)
    {
        delegatePairs[playerAction] -= someEvent;
        delegatePairs[playerAction] += someEvent;
    }

    public InputAction GetAction(PlayerAction playerAction)
    {
        if (actionPairs.TryGetValue(playerAction, out InputAction action))
            return action;
        else
            return null;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        delegatePairs[PlayerAction.Move]?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        delegatePairs[PlayerAction.Interact]?.Invoke(context);
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        delegatePairs[PlayerAction.Skip]?.Invoke(context);
    }
    public bool IsKeyTriggered(PlayerAction action)
    {
        if (GetAction(action).triggered)
            return true;
        else
            return false;
    }

    public void OnEnable()
    {
        if (isInitialized)
        {
            foreach (var action in actionPairs.Values)
            {
                action.Enable();
            }
        }
    }

    public void OnDisable()
    {
        if (isInitialized)
        {
            foreach (var action in actionPairs.Values)
            {
                action.Disable();
            }
        }
    }
}
