using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
public class PlayerProfile : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI userNimText;
    public TextMeshProUGUI userKodeKampusText;
    public TextMeshProUGUI userProdiText;
    public TextMeshProUGUI userKodeProdiText;
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update(){
        UpdatePlayerProfile();
    }

    void UpdatePlayerProfile()
    {
        if (gameManager != null)
        {
            // GameManager.CreatePlayer += UpdatePlayerProfile;
            // Initially update the profile if a player already exists
            DisplayPlayerData();
        }
    }

    void DisplayPlayerData()
    {
        if (gameManager != null && IsPlayerDataValid(gameManager.player))
        {
             usernameText.text =gameManager.player.nickname;
            userNimText.text = gameManager.player.uuid;
            userKodeKampusText.text =  gameManager.player.kodeuniv;
            userProdiText.text = gameManager.player.fakultas;
            userKodeProdiText.text =  gameManager.player.jurusan;
        }
        else
        {
            Debug.LogError("Player data not found!");
        }
    }

    bool IsPlayerDataValid(MainMenu.PlayerData playerData)
    {
        // Check for essential fields to determine if player data is valid
        return !string.IsNullOrEmpty(playerData.uuid) &&
               !string.IsNullOrEmpty(playerData.nickname) &&
               !string.IsNullOrEmpty(playerData.role) &&
               !string.IsNullOrEmpty(playerData.gender) &&
               !string.IsNullOrEmpty(playerData.universitas) &&
               !string.IsNullOrEmpty(playerData.kodeuniv) &&
               !string.IsNullOrEmpty(playerData.fakultas) &&
               !string.IsNullOrEmpty(playerData.jurusan);
    }
}