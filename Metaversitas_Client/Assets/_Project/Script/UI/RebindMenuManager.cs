using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindMenuManager : MonoBehaviour
{
    public InputActionReference MoveRef, JumRef, FireRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        MoveRef.action.Disable();
        /*JumRef.action.Disable();
        FireRef.action.Disable();*/
    }

    private void OnDisable()
    {
        MoveRef.action.Enable();
        /*JumRef.action.Enable();
        FireRef.action.Enable();*/

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
