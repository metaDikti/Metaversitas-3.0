using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputReceiver : MonoBehaviour
{
    private PlayerInputAction controls;
    [SerializeField] private Vector2 look;
    [SerializeField] private Vector2 move;
    public float sensitivity = 2.0f;

    private void Awake()
    {
        controls = new PlayerInputAction();
        controls.Player.Move.performed += ctx => OnMovePerformed(ctx);
        controls.Player.Move.canceled += ctx => OnMoveCanceled(ctx);
        controls.Player.Look.performed += ctx => OnLookPerformed(ctx);
        controls.Player.Look.canceled += ctx => OnLookCanceled(ctx);
        controls.Player.Interact.performed += ctx => OnInteractPerformed(ctx);
        controls.Player.Pause.performed += ctx => OnPausePerformed(ctx);
        controls.Player.Sprint.performed += ctx => OnSprintPerformed(ctx);
        controls.Player.Sprint.canceled += ctx => OnSprintCanceled(ctx);
        controls.Player.Tab.performed += ctx => OnTabPerformed(ctx);
        controls.Player.Enter.performed += ctx => OnEnterPerformed(ctx);
        controls.Player.Escape.performed += ctx => OnEscapePerformed(ctx);
    }
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Interact.Enable();
        controls.Player.Sprint.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Interact.Disable();
        controls.Player.Sprint.Disable();
    }
    private void OnTabPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GeneralMenu.Instance.MenuActive()) return;

            if (UIChatController.Instance._chatInput.isFocused)
            {
                UIChatController.Instance.chatInputActive();
                UIChatController.Instance.chatNameInputActive();
                UIChatController.Instance._chatNameInput.Select();
            }
            else
            {
                UIChatController.Instance.chatNameInputActive();
                LocalController.Instance.Toggle();
            }
        }
    }
    private void OnEnterPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GeneralMenu.Instance.MenuActive() == true) return;
            // Ensure the chat box is visible
            UIChatController.Instance._canvasGroup.alpha = 1;

            if (UIChatController.Instance._chatNameInput.isFocused)
            {
                UIChatController.Instance.chatNameInputActive();
                UIChatController.Instance.chatInputActive();
                UIChatController.Instance._chatInput.Select();
            }
            else
            {
                UIChatController.Instance.SendChatMessage();
                UIChatController.Instance.chatInputActive();
                LocalController.Instance.Toggle();
            }
        }
    }
    private void OnEscapePerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (UIChatController.Instance._chatInput.isFocused || UIChatController.Instance._chatNameInput.isFocused)
            {
                UIChatController.Instance.HideChatBox();
                LocalController.Instance.Toggle();
            }
            else if(Interactable.ActiveInteractable != null)
            {
                Interactable.ActiveInteractable.Close();
            } else if (CutsceneManager.Cutscene != null)
            {
                CutsceneManager.Cutscene.SkipVideo();
            }
            else
            {
                GeneralMenu.Instance.Toggle();
                LocalController.Instance.Toggle();
            }
        }
    }

    void OnEscape(InputValue value)
    {
        
    }

    void OnLook(InputValue value)
    {
        
    }

    void OnPause(InputValue value) 
    {
        
    }

    void OnSprint(InputValue value) 
    {
        
    }

    void OnInteract(InputValue value)
    {

    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.isLooking = true;
        look = ctx.ReadValue<Vector2>() * sensitivity;
        LocalController.Instance.look = look;
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        look = Vector2.zero;
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.isLooking = true;
        LocalController.Instance.look = look;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.SetMove(move);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        move = Vector2.zero;
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.SetMove(move);
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact button 'E' pressed!");
        LocalController.Instance.Interact();
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Esc pressed");
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        if (LocalController.Instance == null)
        {
            return;
        }
        LocalController.Instance.isSprinting = false;
    }

}
