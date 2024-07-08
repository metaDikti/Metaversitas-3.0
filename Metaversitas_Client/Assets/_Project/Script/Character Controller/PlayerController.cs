using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private float modelTurnSpeed;
    public float currentRotation = 0f;
    public float rotationDelta;
    public float speed = 10;
    public float sprintSpeedMultiplier = 2.0f;
    private Transform playerCamera;
    public bool moving;
    public bool isLooking;
    public bool isSprinting;
    public Vector3 movement;
    private Rigidbody body;
    public bool isPaused;
    public AnimasiController PlayerAnimator;

    private void Start()
    {
        playerCamera = Camera.main.transform;
        body = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponentInChildren<AnimasiController>(true);
    }

    public Transform GetModelTransform() => modelTransform;

    private void FixedUpdate()
    {
        if (isPaused) return;
        body.MovePosition(body.position + (movement * (Time.fixedDeltaTime * sprintSpeedMultiplier)));
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    private void Update()
    {
        PlayerAnimator.running = movement.magnitude;
    }
}
