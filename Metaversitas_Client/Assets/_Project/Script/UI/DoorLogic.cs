using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public Pertemuan _pertemuan;  
    public GameObject doorOpen;   
    public GameObject doorClosed; 

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager instance in the scene
        gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            UpdateDoorState();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDoorState();
    }

    void UpdateDoorState()
    {
        if (gameManager._pertemuan == _pertemuan.ToString())
        {
            doorOpen.SetActive(true);
            doorClosed.SetActive(false);
        }
        else
        {
            doorOpen.SetActive(false);
            doorClosed.SetActive(true);
        }
    }
}
