using SpacetimeDB.Types;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class UIChatController : MonoBehaviour
{
    public static UIChatController Instance { get; private set; }
    public GameManager gameManager;
    public TMP_InputField _chatNameInput;
    public TMP_InputField _chatInput;
    public TMP_Text _messages;
    public ScrollRect _scrollRect;
    public CanvasGroup _canvasGroup;
    private float _idleTime = 5.0f;
    [SerializeField] private float _timer = 5.0f;
    public Lobby lobby;
    public PlayerComponent _localPlayer;
    public ulong _localplayerId;
    public string _localplayerName;
    private ulong _targetId;
    private bool messageActive;
    private bool nameActive;

    private void Awake()
    {
        _canvasGroup.alpha = 1;
    }
    void Start()
    {
        Instance = this;
        gameManager = GameManager.Instance;
        if (!gameManager._multiPlayer)
        {
            gameObject.SetActive(false);
        }
        lobby = gameManager._currentLobby;
        _localPlayer = gameManager._localPlayer;
        _localplayerId = _localPlayer.EntityId;
        _localplayerName = _localPlayer.Nickname;
        _messages.text = "";
        _chatInput.text = "";
        _chatNameInput.text = "";
        _chatInput.DeactivateInputField();
        _chatNameInput.DeactivateInputField();
    }

    void Update()
    {
        if (_timer > 0 )
        {
            _timer -= Time.deltaTime;
        } else if (_timer <= 0)
        {
            _canvasGroup.alpha = 0;
            ResetIdleTimer();
        }

        if ( messageActive || nameActive) { _canvasGroup.alpha = 1; }
        ToggleChatInput();
        ToggleChatNameInput();
        
    }
    public void OnChatMessageReceived(string message)
    {
        _messages.text += $"{message}\n";
        Debug.Log(message);
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    public void chatInputActive()
    {
        messageActive = !messageActive;
    }
    public void chatNameInputActive()
    {
        nameActive = !nameActive;
    }

    public void ToggleChatInput()
    {
        
        if(messageActive)
        {
            _chatInput.ActivateInputField(); // Set focus on the chat input field
            _chatInput.Select();
        } else
        {
            _chatInput.DeactivateInputField();
        }
    }
    public void ToggleChatNameInput()
    {
        if (nameActive)
        {
            _chatNameInput.ActivateInputField();
            _chatNameInput.Select();
        }
        else
        {
            _chatNameInput.DeactivateInputField();
        }
    }

    private void ResetIdleTimer()
    {
        _timer = _idleTime;
    }
    public void HideChatBox()
    {
        _canvasGroup.alpha = 0; // Set alpha to 0 to hide the chatbox
        nameActive = false;
        messageActive = false;
    }

    public void SendChatMessage()
    {
        string playerTarget = _chatNameInput.text;
        Debug.Log(playerTarget);
        string message = _chatInput.text; // Get the text from the input field
        if (string.IsNullOrEmpty(playerTarget)) 
        {
            if (!string.IsNullOrEmpty(message))
            {
                Reducer.PublicChatMessage(lobby.Id, $"Public: {message}"); // Send the message
                _chatInput.text = ""; // Clear the input field
                ResetIdleTimer();
                Debug.Log("Public");
            }
        } else 
        {
            if (!string.IsNullOrEmpty(message))
            {
                _targetId = gameManager.GetOwnerIdByNickname(playerTarget);
                if ( _targetId == 0)
                {
                    Debug.Log("Tidak ada User");
                    OnChatMessageReceived("Tidak ada User");
                }
                Reducer.SendPrivateMessage(lobby.Id, _localplayerId, _targetId, $"{_localplayerName}: {message}");
                _chatInput.text = "";
                _chatNameInput.text = "";
                ResetIdleTimer();
                Debug.Log("Private");
            } 
            return;
        }
    }
}