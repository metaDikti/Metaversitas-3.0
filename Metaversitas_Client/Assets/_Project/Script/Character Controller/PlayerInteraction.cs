using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    public Interactable currentInteractable;
    public LayerMask interactionMask;
    public LayerMask limitMask;
    public Transform aim;
    public GameObject interactNotification;
    public GameObject limitNotification;

    private void Start()
    {
        interactNotification.SetActive(false);
        limitNotification.SetActive(false);
    }

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(aim.position, aim.forward * 5, Color.blue);

        if (Physics.Raycast(aim.position, aim.forward, out hit, 5, interactionMask))
        {
            HideLimitNotification();
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                    ShowInteractionNotification();
                }
            }
            else
            {
                ClearInteractionInteractable();
            }
        }
        else if (Physics.Raycast(aim.position, aim.forward, out hit, 5, limitMask))
        {
            ShowLimitNotification();
            ClearInteractionInteractable();
        }
        else
        {
            ClearInteractionInteractable();
            HideLimitNotification();
        }
    }

    void ShowInteractionNotification()
    {
        if (!interactNotification.activeSelf)
        {
            interactNotification.SetActive(true);
        }
    }

    void ShowLimitNotification()
    {
        if (!limitNotification.activeSelf)
        {
            limitNotification.SetActive(true);
        }
    }

    void HideLimitNotification()
    {
        if (limitNotification.activeSelf)
        {
            limitNotification.SetActive(false);
        }
    }

    void HideInteractionNotification()
    {
        if (interactNotification.activeSelf)
        {
            interactNotification.SetActive(false);
        }
    }

    void ClearInteractionInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            HideInteractionNotification();
        }
    }

    public void CheckInteractionInput()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
