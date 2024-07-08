using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpacetimeDB;
using SpacetimeDB.Types;

public class LobbyButton : MonoBehaviour
{
    public TextMeshProUGUI lobbyText;
    public Lobby lobby;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void SetupLobbyUI(Lobby lobbyToSetup)
    {
        lobby = lobbyToSetup; // Memperbaiki inisialisasi variabel lobby
        // Assuming you have UI elements to display lobby details, you can set their text or values here
        if (lobbyText != null)
        {
            // Set the text of the TextMeshPro component with lobby details
            lobbyText.text = $"Lobby {lobby.Kelas} - {lobby.Materi} - {lobby.Pertemuan}";
        }
        // Add other setup code here as needed
    }

    public void Joinbutton()
    {
        gameManager._currentLobby = lobby;
        gameManager._multiPlayer = true;
        gameManager.OnLobbyUpdate();
    }
}
