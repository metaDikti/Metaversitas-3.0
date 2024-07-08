using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private Transform lobbyStorage;

    public void DestroyAllChildren()
    {
        foreach (Transform child in lobbyStorage)
        {
            Destroy(child.gameObject);
        }
    }
}
