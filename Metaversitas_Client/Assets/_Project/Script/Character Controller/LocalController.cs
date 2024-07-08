using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SpacetimeDB.Types;
using Unity.Mathematics;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class LocalController : MonoBehaviour
{
    public ulong EntityId { get; set; }

    public static LocalController Instance;
    public string Username;

    public TMP_Text UsernameElement;

    public bool isLooking;
    public Vector2 move;
    public Vector2 look;
    private Vector2 movementVec;
    public float speed = 10;
    public float sprintSpeedMultiplier = 2.0f;
    public float mouseSensitivity = 100f;
    public float mouseX;
    public float currentRotation = 0f;
    float rotationDelta;
    [SerializeField] private float rotationSpeed = 1.0f;

    public bool isSprinting;
    public PlayerController playerController;
    public MouseLook mouseLook;
    public PlayerInteraction playerInteraction;
    public AnimasiController PlayerAnimator;
    private bool isPaused = false;
    public bool isSpawned;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        playerController = GetComponent<PlayerController>();
        mouseLook = GetComponentInChildren<MouseLook>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        PlayerAnimator = GetComponentInChildren<AnimasiController>(true);
        UsernameElement = GetComponentInChildren<TMPro.TMP_Text>();
        UsernameElement.text = Username;
        spawnPoint = gameObject.transform;
    }

    public void Toggle()
    {
        isPaused = !isPaused;
    }

    private Vector3? lastUpdateDirection;
    private float? lastUpdateRotation;

    public void SetMove(UnityEngine.Vector3 vec) => movementVec = vec;
    public void SetRotation(UnityEngine.Vector3 vec) => look = vec;

    public Vector3 GetDirectionVec()
    {
        var vec = new Vector3(movementVec.x, 0, movementVec.y);
        return mouseLook.transform.TransformDirection(vec);
    }

    public void Update()
    {
        if (isPaused)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            mouseLook.look = Vector2.zero;
        } else
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            mouseLook.look = look;
            // Assuming 'rotationDelta' is your float value from input or another source
            rotationDelta = look.x * rotationSpeed * Time.deltaTime; // Send it to the server
            currentRotation += rotationDelta;
        }
        playerController.isPaused=isPaused;
    }

    public void Spawning(Transform randomSpawnPoint)
    {
        gameObject.transform.position = new Vector3(randomSpawnPoint.position.x, 0.0f, randomSpawnPoint.position.z);
        Reducer.MovePlayer(new StdbVector3() { X = transform.position.x, Y = transform.position.y, Z = transform.position.z }, new StdbVector2() { X = 0, Z = 0 }, true, false);
        isSpawned = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isSpawned) return;
        var directionVec = GetDirectionVec();
        if (isSprinting)
        {
            directionVec = directionVec * sprintSpeedMultiplier;
        }
        playerController.movement = directionVec;
        bool moving = directionVec.sqrMagnitude > 0;
        playerController.moving = moving;

        var ourPos = playerController.GetModelTransform().position;

        if (directionVec.magnitude != 0 && (!lastUpdateDirection.HasValue || (directionVec - lastUpdateDirection.Value).sqrMagnitude > .1f))
        {
            if (!isSprinting)
            {
                Reducer.MovePlayer(new StdbVector3() { X = ourPos.x, Y = ourPos.y, Z = ourPos.z }, new StdbVector2() { X = directionVec.x, Z = directionVec.z }, true, false);
            }
            else
            {
                Reducer.MovePlayer(new StdbVector3() { X = ourPos.x, Y = ourPos.y, Z = ourPos.z }, new StdbVector2() { X = directionVec.x, Z = directionVec.z }, false, true);
            }
            lastUpdateDirection = directionVec;
        }
        else if (directionVec.magnitude == 0)
        {
            Reducer.StopPlayer(new StdbVector3() { X = ourPos.x, Y = ourPos.y, Z = ourPos.z }, new StdbVector2() { X = directionVec.x, Z = directionVec.z });
            lastUpdateDirection = null;
        }

        var rotationvar = currentRotation;
        var ourRot = playerController.GetModelTransform().rotation;
        playerController.currentRotation = rotationvar;

        if ((rotationvar != 0.0f && (!lastUpdateDirection.HasValue || Mathf.Abs(rotationvar - lastUpdateDirection.Value.magnitude) > 0.1f)))
        {
            Reducer.RotatePlayer(new StdbQuaternion() { X = ourRot.x, Y = ourRot.y, Z = ourRot.z, W = ourRot.w }, rotationvar);
            lastUpdateRotation = rotationvar;
        }
        else if (rotationvar == 0.0f)
        {
            Reducer.StopRotatePlayer(new StdbQuaternion() { X = ourRot.x, Y = ourRot.y, Z = ourRot.z, W = ourRot.w }, rotationvar);
            lastUpdateRotation = null;
        }
    }

    public void Interact()
    {
        if (isPaused) return;
        PlayerAnimator.intreact();
        playerInteraction.CheckInteractionInput();
        Reducer.Interact(EntityId);
    }
}
