using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Linq;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager;
    public bool is_Online;
    public uint Id;
    public string kelas = "ExampleClass";
    public string materi = "ExampleMaterial";
    public string pertemuan = "ExampleMeeting";

    public string filter_kelas = "ExampleClass";
    public string filter_materi = "ExampleMaterial";
    public string filter_pertemuan = "ExampleMeeting";

    public GameObject mainMenu;
    public Transform lobbyMenu;
    public GameObject lobbyUI;
    public GameManager gameManager;

    public PlayerData player;

    // Start is called before the first frame update
    void Start()
    {
        Lobby.OnInsert += OnLobbyInsert;
        Lobby.OnUpdate += OnLobbyUpdate;
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            is_Online = gameManager._connected;
        }
        if (is_Online == true)
        {
            OpenMainMenu();
        }
        GameManager.OnConnected += DoSomethingOnConnect;
    }

    private void DoSomethingOnConnect()
    {
        is_Online = gameManager._connected;
        _menuManager.OpenMenu("login");
    }

    private void OpenMainMenu()
    {
        _menuManager.OpenMenu("main menu");
    }

    public void CreatePlayer()
    {
        if (!is_Online) { return; }
        gameManager.CreatePlayer(player);
    }

    public void CreateLobby() // This method should be called when the create lobby button is pressed.
    {
        if (!is_Online) { return; }
        var participants = new List<ulong>();
        SpacetimeDB.Types.Reducer.CreateLobby(Id, kelas, materi, pertemuan, participants);
        Debug.Log(kelas + materi + pertemuan);
    }

    public void Matchmaking()
    {
        if (!is_Online) { return; }
        SpacetimeDB.Types.Reducer.Matchmaking(filter_kelas, filter_materi, filter_pertemuan);
    }

    public void FilterLobby()
    {
        if (!is_Online) { return; }
        Debug.Log("Calling Filter Function");
        SpacetimeDB.Types.Reducer.FilterLobbies(filter_kelas, filter_materi, filter_pertemuan);
        foreach (var lobby in Lobby.FilterByKelas(filter_kelas).Where(l => l.Materi == filter_materi && l.Pertemuan == filter_pertemuan))
        {
            // Use Debug.Log to log the information about the filtered lobbies in Unity, for example
            Debug.Log($"Lobby: {lobby.Id}, Kelas: {lobby.Kelas}, Materi: {lobby.Materi}, Pertemuan: {lobby.Pertemuan}");
            GameObject lobbies_button = Instantiate(lobbyUI, lobbyMenu);
            LobbyButton lobby_Button = lobbies_button.GetComponent<LobbyButton>();
            if (lobby_Button != null)
            {
                // Call the SetupLobbyUI method to pass the lobby data
                lobby_Button.SetupLobbyUI(lobby);
            }
        }
        mainMenu.SetActive(false);
    }

    public void DeleteLobby(Lobby lobby)
    {
        if (!is_Online) { return; }

        SpacetimeDB.Types.Reducer.DestroyLobby(lobby.Kelas, lobby.Materi, lobby.Pertemuan);
    }


    // Method to handle new lobby inserted
    private void OnLobbyInsert(Lobby lobby, ReducerEvent? reducerEvent)
    {
        // Handle new lobby inserted. E.g., update your UI with new lobby data.
        Debug.Log($"New Lobby Inserted: Kelas: {lobby.Kelas}, Materi: {lobby.Materi}, Pertemuan: {lobby.Pertemuan}");
    }

    // Method to handle lobby update
    private void OnLobbyUpdate(Lobby oldLobby, Lobby newLobby, ReducerEvent reducerEvent)
    {
        // Handle lobby update. E.g., refresh the lobby data in your UI.
        Debug.Log($"Lobby Updated: Open: Kelas: {newLobby.Kelas}, Materi: {newLobby.Materi}, Pertemuan: {newLobby.Pertemuan}");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        
    }

    [System.Serializable]
    public struct PlayerData
    {
        public string role;
        public string uuid;
        public string full_name;
        public string nickname;
        public string gender;
        public string universitas;
        public string kodeuniv;
        public string fakultas;
        public string jurusan;

        // Constructor
        public PlayerData(string role, string uuid, string full_name, string nickname, string gender,
                          string universitas, string kodeuniv, string fakultas, string jurusan)
        {
            this.role = role;
            this.uuid = uuid;
            this.full_name = full_name;
            this.nickname = nickname;
            this.gender = gender;
            this.universitas = universitas;
            this.kodeuniv = kodeuniv;
            this.fakultas = fakultas;
            this.jurusan = jurusan;
        }
    }

}
