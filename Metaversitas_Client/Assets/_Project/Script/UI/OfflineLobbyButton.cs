using SpacetimeDB.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfflineLobbyButton : MonoBehaviour
{
    public Lobby lobby;
    public Materi _materi;
    public Pertemuan _pertemuan;
    public GameObject searchedObject;
    private GameManager gameManager;
    public TextMeshProUGUI offlineLobbyText;
    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.Instance;
        offlineLobbyText.text = $"Lobby {_materi} - {_pertemuan}";
    }

    private void Update()
    {
        if (gameManager == null)
        {
            searchedObject = GameObject.Find("GameManager(Clone)");
            gameManager = searchedObject.GetComponent<GameManager>();
        }
        offlineLobbyText.text = $"Lobby {_materi} - {_pertemuan}";
    }

    public void Joinbutton()
    {
        gameManager._materi = _materi.ToString();
        gameManager._pertemuan = _pertemuan.ToString();
        gameManager._multiPlayer = false;
        gameManager._singlePlayer = true;
        gameManager.OnLobbyUpdate();
    }
}
