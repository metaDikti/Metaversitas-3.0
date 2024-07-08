using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Linq;
using static MainMenu;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
using ClientApi;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _connectingScreen;
    // These are connection variables that are exposed on the GameManager
    // inspector. The cloud version of SpacetimeDB needs sslEnabled = true
    [SerializeField] private string moduleAddress = "YOUR_MODULE_DOMAIN_OR_ADDRESS";
    [SerializeField] private string hostName = "localhost:3000";
    [SerializeField] private bool sslEnabled = false;

    // This is the identity for this player that is automatically generated
    // the first time you log in. We set this variable when the 
    // onIdentityReceived callback is triggered by the SDK after connecting
    private Identity local_identity;
    public PlayerComponent _localPlayer;
    public Lobby _currentLobby;
    public string _materi;
    public string _pertemuan;

    public static GameManager Instance { get; private set; }
    public delegate void OnConnectedHandler();
    public static event OnConnectedHandler OnConnected;
    public GameObject spawnpointObject;
    private Spawnpoint spawnpoint;
    private Transform randomSpawnPoint;
    private MapLoading mapLoading;
    public HashSet<Identity> _currentParticipants = new HashSet<Identity>();
    public HashSet<PlayerComponent> _currentParticipantsComponent = new HashSet<PlayerComponent>();
    public event Action<HashSet<Identity>> OnParticipantsUpdated;
    private HashSet<Identity> spawnedPlayers = new HashSet<Identity>();
    public MapIndex mapIndex;
    private Scene currentScene;
    public PlayerData player;
    public bool _connected;
    public bool _multiPlayer;
    public bool _singlePlayer;
    public GameObject _mMLocalController;
    public GameObject _fMLocalController;
    public GameObject _mDLocalController;
    public GameObject _fDLocalController;
    public LocalController LocalController;
    private bool inGameScene = false;

    void Awake()
    {
        if (Instance == null)
        {
            // This GameObject instance will persist across scene changes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // If another instance already exists, destroy this GameObject
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _connectingScreen.SetActive(true);
        mapLoading = GetComponent<MapLoading>();
        Application.runInBackground = true;

        SpacetimeDBClient.instance.onConnect += () =>
        {
            _connected = true;
            _connectingScreen.SetActive(false);

            // Request all tables
            SpacetimeDBClient.instance.Subscribe(new List<string>()
        {
            "SELECT * FROM Config",
            "SELECT * FROM Lobby",
            "SELECT * FROM PlayerComponent",
            "SELECT * FROM SpawnableEntityComponent",
            "SELECT * FROM MobileEntityComponent",
            "SELECT * FROM AnimationComponent",
            "SELECT * FROM ChatMessage",
        });
            // Invoke the OnConnected event
            OnConnected?.Invoke();
        };

        // Called when we have an error connecting to SpacetimeDB
        SpacetimeDBClient.instance.onConnectError += (error, message) =>
        {
            Debug.LogError($"Connection error: " + message);
        };

        // Called when we are disconnected from SpacetimeDB
        SpacetimeDBClient.instance.onDisconnect += (closeStatus, error) =>
        {
            Debug.Log("Disconnected.");
        };

        // Called when we receive the client identity from SpacetimeDB
        SpacetimeDBClient.instance.onIdentityReceived += (token, identity, address) =>
        {
            AuthToken.SaveToken(token);
            local_identity = identity;
            _localPlayer = PlayerComponent.FilterByOwnerId(local_identity);
            Debug.Log($" Starting" + _localPlayer);
        };

        PlayerComponent.OnUpdate += (oldValue, newValue, dbEvent) =>
        {
            // Handle the update, e.g., update the UI or game state
            Debug.Log($"Player in_game status updated: {newValue.InGame}");
        };
        SpacetimeDBClient.instance.onSubscriptionApplied += OnSubscriptionApplied;

        PlayerComponent.OnUpdate += PlayerComponent_OnUpdate;
        PlayerComponent.OnInsert += PlayerComponent_OnInsert;
        AnimationComponent.OnInsert += OnAnimationComponentInsert;
        AnimationComponent.OnUpdate += OnAnimationComponentUpdate;
        Reducer.OnInteractEvent += OnInteractEvent;
        SpacetimeDB.Types.Reducer.OnMatchmakingEvent += HandleMatchmakingCompleted;
        Lobby.OnUpdate += OnLobbyUpdate;
        Reducer.OnSendPrivateMessageEvent += Reducer_OnSendPrivateMessageEvent;
        Reducer.OnPublicChatMessageEvent += Reducer_OnPublicChatMessageEvent;


        // Now that we’ve registered all our callbacks, lets connect to spacetimedb
        SpacetimeDBClient.instance.Connect(AuthToken.Token, hostName, moduleAddress);
        currentScene = SceneManager.GetActiveScene();

    }

    private void Reducer_OnPublicChatMessageEvent(ReducerEvent reducerEvent, uint lobbyId, string message)
    {
        var player = PlayerComponent.FilterByOwnerId(reducerEvent.Identity);
        if (player != null && lobbyId == _currentLobby.Id)
        {
            UIChatController.Instance.OnChatMessageReceived(player.Nickname + ": " + message);
            Debug.Log($"Public" + message);
        }
    }

    private void Reducer_OnSendPrivateMessageEvent(ReducerEvent reducerEvent, uint lobbyId, ulong senderId, ulong recipientId, string message)
    {
        var player = PlayerComponent.FilterByOwnerId(reducerEvent.Identity);
        if (player != null && lobbyId == _currentLobby.Id && recipientId == _localPlayer.EntityId)
        {
            UIChatController.Instance.OnChatMessageReceived(player.Nickname + ": " + message);
            Debug.Log($"Private" + message);
        }
    }

    private void OnInteractEvent(ReducerEvent reducerEvent, ulong entityId)
    {
        if (reducerEvent.Status == ClientApi.Event.Types.Status.Committed && reducerEvent.Identity != local_identity)
        {
            var remotePlayer = FindObjectsOfType<RemotePlayer>().FirstOrDefault(item => item.EntityId == entityId);
            if (remotePlayer != null)
            {
                remotePlayer.Interact();
            }
        }
    }
    private void HandleMatchmakingCompleted(ReducerEvent reducerEvent, string kelas, string materi, string pertemuan)
    {
        // Logika yang ingin dijalankan ketika matchmaking selesai
        Console.WriteLine($"Matchmaking completed for Kelas: {kelas}, Materi: {materi}, Pertemuan: {pertemuan}");
        SearchLobby(kelas, materi, pertemuan);
        _multiPlayer = true;
        OnLobbyUpdate();

        // Contoh tambahan logika lain, misalnya memperbarui UI atau mengirim notifikasi
        // UpdateUI(kelas, materi, pertemuan);
        // SendNotification(kelas, materi, pertemuan);
    }

    void SearchLobby(string kelas, string materi, string pertemuan)
    {
        SpacetimeDB.Types.Reducer.FilterLobbies(kelas, materi, pertemuan);
        // Filter lobbies berdasarkan kelas, materi, dan pertemuan
        var filteredLobbies = Lobby.FilterByKelas(kelas)
            .Where(l => l.Materi == materi && l.Pertemuan == pertemuan);

        // Ambil lobby pertama yang sesuai dengan filter
        var lobby = filteredLobbies.FirstOrDefault();

        // Pastikan lobby tidak null sebelum mengaksesnya
        if (lobby != null)
        {
            _currentLobby = lobby;
        }
        else
        {
            // Handle jika tidak ada lobby yang ditemukan
            Debug.LogWarning("No lobby found with the specified criteria.");
        }
    }

    public void Join()
    {
        Debug.Log($"Lobby {_currentLobby.Kelas} - {_currentLobby.Materi} - {_currentLobby.Pertemuan}");
        SpacetimeDB.Types.Reducer.JoinLobby(_currentLobby.Kelas, _currentLobby.Materi, _currentLobby.Pertemuan);
        _materi = _currentLobby.Materi;
        _pertemuan = _currentLobby.Pertemuan;
    }

    private void CheckActiveScene()
    {
        inGameScene = SceneManager.GetActiveScene().name != "MainMenu";
    }

    public bool IsInGameScene()
    {
        return inGameScene;
    }

    void Update()
    {
        // Optionally, you can keep checking in Update if the scene might change during gameplay.
        CheckActiveScene();
    }

    private void OnLobbyUpdate(Lobby oldLobby, Lobby newLobby, ReducerEvent reducerEvent)
    {
        // Handle lobby update. E.g., refresh the lobby data in your UI.
        //Debug.Log($"Lobby Updated: Open: Kelas: {newLobby.Kelas}, Materi: {newLobby.Materi}, Pertemuan: {newLobby.Pertemuan}, Participan: {newLobby.Participants}, Time Limit: {newLobby.TimeLimit}");
        if (_currentLobby != null && newLobby.Id == _currentLobby.Id)
        {

            HashSet<Identity> combinedParticipants = new HashSet<Identity>(oldLobby.Participants);
            combinedParticipants.UnionWith(newLobby.Participants);
            Debug.Log($"All Participan {combinedParticipants}");

            // Identify and add new participants
            foreach (var participant in combinedParticipants)
            {
                if (!_currentParticipants.Contains(participant) && participant != local_identity)
                {
                    Debug.Log("New Participant: " + participant);
                    AddSpawnedParticipant(participant);
                }
            }

            // Identify and remove participants who have left
            var participantsLeft = _currentParticipants.Except(combinedParticipants).ToList();
            foreach (var participantId in participantsLeft)
            {
                Debug.Log("Participant Left: " + participantId);
                RemoveSpawnedParticipant(participantId);
                var player = PlayerComponent.FilterByOwnerId(participantId);
                DispawnRemoteCharacterController(player);
                spawnedPlayers.Remove(participantId);
            }

            // Update the currentParticipants set to match the new state
            UpdateParticipants(combinedParticipants);
            CallSpawnedForRemoteController();

            if(newLobby.TimeLimit == 600)
            {
                Debug.Log("10 Menit lagi");
            } else if (newLobby.TimeLimit == 300)
            {
                Debug.Log("5 Menit lagi");
            } else if (newLobby.TimeLimit <= 2)
            {
                Debug.Log("Leave");
                LeaveLobby();
            }
        }
    }

    private void UpdateParticipants(HashSet<Identity> updatedParticipants)
    {
        _currentParticipants = updatedParticipants;
        foreach ( var participants in updatedParticipants)
        {
            QueryPlayerComponentByEntityId(participants);
        }
        OnParticipantsUpdated?.Invoke(_currentParticipants);
    }

    public void QueryPlayerComponentByEntityId(SpacetimeDB.Identity ownerId)
    {
        PlayerComponent playerComponent = PlayerComponent.FilterByOwnerId(ownerId);
        if (playerComponent != null)
        {
            if (_currentParticipantsComponent.Add(playerComponent))
            {
                Debug.Log($"Added player component with entity ID {ownerId}: {playerComponent.FullName}");
            }
            else
            {
                Debug.Log($"Player component with entity ID {ownerId} already exists in the collection.");
            }
        }
        else
        {
            Debug.Log($"No player component found with entity ID {ownerId}");
        }
    }

    public ulong GetOwnerIdByNickname(string nickname)
    {
        foreach (var playerComponent in _currentParticipantsComponent)
        {
            if (playerComponent.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Found player with nickname {nickname}: {playerComponent.OwnerId}");
                return playerComponent.EntityId;
            }
        }
        Debug.Log($"No player found with nickname {nickname}");
        return 0; // or another representation of 'not found'
    }

    public void LeaveLobby()
    {
        LeaveLobby(_currentLobby);
        _currentLobby = null;
        _multiPlayer = false;
        _singlePlayer = false;
        _materi = null;
        _pertemuan = null;
        RemoveAllParticipant();
        RemoveAllSpawnParticipant();
        OnLobbyUpdate();
    }

    void LeaveLobby(Lobby lobbyJoined)
    {
        if (_currentLobby != null)
        {
            SpacetimeDB.Types.Reducer.LeaveLobby(lobbyJoined.Kelas, lobbyJoined.Materi, lobbyJoined.Pertemuan);
        }
        
    }

    public void RemoveAllParticipant()
    {
        _currentParticipants.Clear();
    }

    public void RemoveAllSpawnParticipant()
    {
        spawnedPlayers.Clear();
    }

    public void AddSpawnedParticipant(Identity ownerId)
    {
        // Adds the ownerId to the collection if it's not already present
        bool added = _currentParticipants.Add(ownerId);
        if (added)
        {
            Debug.Log($"Participant with OwnerId {ownerId} added.");
        }
        else
        {
            Debug.Log($"Participant with OwnerId {ownerId} was already added.");
        }
    }

    public void RemoveSpawnedParticipant(Identity ownerId)
    {
        // Attempts to remove the ownerId from the collection
        bool removed = _currentParticipants.Remove(ownerId);
        if (removed)
        {
            Debug.Log($"Participant with OwnerId {ownerId} removed.");
        }
        else
        {
            Debug.Log($"Attempted to remove Participant with OwnerId {ownerId}, but they were not found in the collection.");
        }
    }

    public void CreatePlayer(PlayerData playerData)
    {
        player = playerData;
        SpacetimeDB.Types.Reducer.CreatePlayer(player.role, player.gender,
                                           player.uuid, player.full_name, player.nickname,
                                           player.universitas, player.kodeuniv, player.fakultas,
                                           player.jurusan);
        _localPlayer = PlayerComponent.FilterByOwnerId(local_identity);
        Debug.Log($"Create Player " + _localPlayer);
    }

    public void UpdateInGameStatus(bool Status)
    {
        SpacetimeDB.Types.Reducer.UpdatePlayerInGameStatus(Status);
    }

    public void OnLobbyUpdate()
    {
            if (_currentLobby == null && _materi == null && _pertemuan == null)
            {
                mapIndex = MapIndex.HomePage;
            }
            else if (_currentLobby == null && _materi != null && _pertemuan != null)
            {
                mapIndex = mapLoading.GetMapIndexByMateri(_materi);
            }
            else 
            {
                mapIndex = mapLoading.GetMapIndexByMateri(_currentLobby.Materi);
            }
            
            // Call LoadSceneByMapIndex with the mapped enum value
            if (mapIndex != MapIndex.HomePage && _multiPlayer == true) // Skip loading HomePage directly
            {
                mapLoading.LoadSceneByMapIndex(mapIndex);
            }
            else if (mapIndex != MapIndex.HomePage && _multiPlayer == false)
            {
                mapLoading.LoadSceneByMapIndex(mapIndex);
            }
            else if (mapIndex == MapIndex.HomePage)
            {
                mapLoading.LoadSceneByMapIndex(mapIndex);
                _multiPlayer = false;
                UpdateInGameStatus(false);
            }
            else
            {
                Debug.LogWarning("Cannot load HomePage scene directly.");
            }
    }


    public void CallUpdateInGameStatus()
    {
        if (_multiPlayer && !_singlePlayer)
        {
            UpdateInGameStatus(true);
            Join();
            Debug.Log("Multiplayer aktif");
        }
        else if (!_multiPlayer && _singlePlayer)
        {
            UpdateInGameStatus(true);
            Debug.Log("Singleplayer aktif");
        }
        else
        {
            UpdateInGameStatus(false);
            Debug.Log("Back To Mainmenu");
        }
    }

    private void OnSubscriptionApplied()
    {
        // If we don't have any data for our player, then we are creating a 
        // new one. Let's show the username dialog, which will then call the
        // create player reducer
        var player = PlayerComponent.FilterByOwnerId(local_identity);
        if (player == null){
            Debug.Log("not Login yet");
        } else
        {
            Debug.Log("Login");
        }
    }

    private void PlayerComponent_OnUpdate(PlayerComponent oldValue, PlayerComponent newValue, ReducerEvent dbEvent)
    {
        Debug.Log("OnUpdate");
        if (oldValue.InGame != newValue.InGame)
        {
            // The in_game variable has changed. React accordingly.
            Debug.Log($"Player {newValue.Nickname} in_game status changed to: {newValue.InGame}");
            OnPlayerComponentChanged(newValue);
        }
    }

    private void PlayerComponent_OnInsert(PlayerComponent obj, ReducerEvent dbEvent)
    {
        SpacetimeDBClient.instance.onSubscriptionApplied += OnSubscriptionApplied;
        Debug.Log($"Player {obj.Nickname} in_game status changed to: {obj.InGame}");
    }

    private void OnPlayerComponentChanged(PlayerComponent obj)
    {
        // Check if the update is for the local player and if they are marked as in_game
        if (obj.OwnerId == local_identity )
        {
            // Spawn or activate the character controller for the local player
            if (obj.InGame)
            {
                    Debug.Log(obj.Nickname + "Ingame");
                    string Gender = obj.Gender.ToString();
                    string Role = obj.Role.ToString();
                    randomSpawnPoint = GeneratedSpawnedPoint();
                    SpawnLocalCharacterController(obj);
                    LocalController.transform.position = new Vector3(randomSpawnPoint.position.x, 0.0f, randomSpawnPoint.position.z);
                    LocalController.Spawning(randomSpawnPoint);
                    LocalController.Username = obj.Nickname;
                    LocalController.EntityId = obj.EntityId;
            } 
        }
    }

    private void SpawnLocalCharacterController(PlayerComponent obj)
    {
        
        // Your logic to spawn or activate the character controller
        if ( obj.Gender == "Male" && obj.Role == "Mahasiswa")
        {
            GameObject localControllerObject = Instantiate(_mMLocalController, randomSpawnPoint.position, Quaternion.identity);
            localControllerObject.AddComponent<LocalController>().EntityId = obj.EntityId;
            LocalController = localControllerObject.GetComponent<LocalController>();
        } else if (obj.Gender == "Female" && obj.Role == "Mahasiswa")
        {
            GameObject localControllerObject = Instantiate(_fMLocalController, randomSpawnPoint.position, Quaternion.identity);
            localControllerObject.AddComponent<LocalController>().EntityId = obj.EntityId;
            LocalController = localControllerObject.GetComponent<LocalController>();
        }
        else if (obj.Gender == "Male" && obj.Role == "Dosen")
        {
            GameObject localControllerObject = Instantiate(_mDLocalController, randomSpawnPoint.position, Quaternion.identity);
            localControllerObject.AddComponent<LocalController>().EntityId = obj.EntityId;
            LocalController = localControllerObject.GetComponent<LocalController>();
        }
        else if (obj.Gender == "Female" && obj.Role == "Dosen")
        {
            GameObject localControllerObject = Instantiate(_fDLocalController, randomSpawnPoint.position, Quaternion.identity);
            localControllerObject.AddComponent<LocalController>().EntityId = obj.EntityId;
            LocalController = localControllerObject.GetComponent<LocalController>();
        }
    }

    private Transform GeneratedSpawnedPoint()
    {
        spawnpointObject = GameObject.Find("Spawn");
        spawnpoint = spawnpointObject.GetComponent<Spawnpoint>();
        randomSpawnPoint = spawnpoint.GetRandomSpawnPoint();
        return randomSpawnPoint;
    }

    private void SpawnRemoteCharacterController(PlayerComponent participant)
    {
        // Your logic to spawn or activate the character controller
        Debug.Log("Remote Player Spawned");
        if (participant.Gender == "Male" && participant.Role == "Mahasiswa")
        {
            var remotePlayer = Instantiate(_mMLocalController);
            remotePlayer.AddComponent<RemotePlayer>().EntityId = participant.EntityId;
        }
        else if (participant.Gender == "Female" && participant.Role == "Mahasiswa")
        {
            var remotePlayer = Instantiate(_fMLocalController);
            remotePlayer.AddComponent<RemotePlayer>().EntityId = participant.EntityId;
        }
        else if (participant.Gender == "Male" && participant.Role == "Dosen")
        {
            var remotePlayer = Instantiate(_mDLocalController);
            remotePlayer.AddComponent<RemotePlayer>().EntityId = participant.EntityId;
        }
        else if (participant.Gender == "Female" && participant.Role == "Dosen")
        {
            var remotePlayer = Instantiate(_fDLocalController);
            remotePlayer.AddComponent<RemotePlayer>().EntityId = participant.EntityId;
        }
    }

    private void CallSpawnedForRemoteController()
    {
        foreach (var participant in _currentParticipants)
        {
            if (participant != local_identity && !spawnedPlayers.Contains(participant) && inGameScene)
            {
                Debug.Log("Spawened: " + participant);
                var player = PlayerComponent.FilterByOwnerId(participant);
                SpawnRemoteCharacterController(player);
                spawnedPlayers.Add(participant);
            }
        }
    }

    private void DispawnRemoteCharacterController(PlayerComponent participant)
    {
        var remotePlayer = FindObjectsOfType<RemotePlayer>().FirstOrDefault(item => item.EntityId == participant.EntityId);
        if (remotePlayer != null)
        {
            Destroy(remotePlayer.gameObject);
        }
    }
    private void OnAnimationComponentInsert(AnimationComponent newValue, ReducerEvent info)
    {
        OnAnimationComponentUpdate(null, newValue, info);
    }

    private void OnAnimationComponentUpdate(AnimationComponent oldValue, AnimationComponent newValue, ReducerEvent info)
    {
        // check to see if this player already exists
        var remotePlayer = RemotePlayer.Players.FirstOrDefault(item => item.EntityId == newValue.EntityId);
        if (remotePlayer)
        {

        }            
    }
}
