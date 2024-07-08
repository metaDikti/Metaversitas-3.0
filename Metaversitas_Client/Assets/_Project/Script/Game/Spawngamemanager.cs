using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawngamemanager : MonoBehaviour
{
    public GameObject gamemanagerPrefab; // The prefab of the GameManager object

    private GameObject searchedObject; // Reference to the searched object
    private GameObject spawnedObject; // Reference to the spawned object

    public MainMenu menu;
    void Start()
    {
        // Search for the "GameManager" object in the scene
        searchedObject = GameObject.Find("GameManager(Clone)");

        if (searchedObject != null)
        {
            // The object was found, you can now work with it
            Debug.Log("GameManager found!");
            menu.gameManager = searchedObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.Log("GameManager not found!");
            SpawnObject();
            menu.gameManager = spawnedObject.GetComponent<GameManager>();
        }
    }

    void SpawnObject()
    {
        // Instantiate the gamemanagerPrefab at the origin (0,0,0) with default rotation
        spawnedObject = Instantiate(gamemanagerPrefab, Vector3.zero, Quaternion.identity);
    }
}

