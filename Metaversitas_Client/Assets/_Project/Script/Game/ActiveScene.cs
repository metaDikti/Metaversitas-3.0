using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ActiveScene : MonoBehaviour
{
    void Start()
    {
        // Get the active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Print the name of the active scene
        Debug.Log("Active Scene: " + currentScene.name);
    }
}
