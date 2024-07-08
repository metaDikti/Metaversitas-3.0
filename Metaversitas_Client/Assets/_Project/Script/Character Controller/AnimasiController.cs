using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AnimasiController : MonoBehaviour
{
    public Animator animator;
    
    public float running;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", running);
    }

    public void intreact()
    {
        animator.SetTrigger("GrabItem");
    }

}
