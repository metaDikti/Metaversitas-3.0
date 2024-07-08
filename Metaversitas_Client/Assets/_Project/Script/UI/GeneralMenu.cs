using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMenu : MonoBehaviour
{
    public static GeneralMenu Instance { get; private set; }
    [SerializeField] private MenuManager _menuManager;
    public GameObject objectToToggle;
    private bool isObjectActive = false;

    public bool MenuActive()
    {
        return isObjectActive;  
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        objectToToggle.SetActive(isObjectActive);
    }

    public void Toggle()
    {
        isObjectActive = !isObjectActive;

        if (objectToToggle != null)
        {
            objectToToggle.SetActive(isObjectActive);
        }
        if (isObjectActive)
        {
            _menuManager.OpenMenu("pausemenu");
        }
    }

    private void Update()
    {

    }
}
