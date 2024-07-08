using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpacetimeDB;
using SpacetimeDB.Types;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    public Lobby lobby;
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _mainMenuButton;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        lobby = gameManager._currentLobby;
        _resumeButton.onClick.AddListener(() =>
        {
            GeneralMenu.Instance.Toggle();
            LocalController.Instance.Toggle();
            _menuManager.OpenMenu("pausemenu");
        });
        _settingButton.onClick.AddListener(() =>
        {
            _menuManager.OpenMenu("setting");
        });
        _mainMenuButton.onClick.AddListener(() =>
         {
             LeaveLobbyButton();
         });
        gameManager = GameManager.Instance;
    }

    void LeaveLobbyButton()
    {
        gameManager.LeaveLobby();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
