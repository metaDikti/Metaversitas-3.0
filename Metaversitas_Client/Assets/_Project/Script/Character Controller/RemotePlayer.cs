using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpacetimeDB.Types;
using TMPro;
using SpacetimeDB;

public class RemotePlayer : MonoBehaviour
{
    public static List<RemotePlayer> Players = new List<RemotePlayer>();

    public ulong EntityId;
    public string Username { set { UsernameElement.text = value; } }

    public TMP_Text UsernameElement;

    void Start()
    {
        DestroyCamera();
        Players.Add(this);

        // initialize overhead name
        UsernameElement = GetComponentInChildren<TMP_Text>();
        var canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        // register for a callback that is called when the client gets an 
        // update for a row in the MobileEntityComponent table
        MobileEntityComponent.OnUpdate += MobileEntityComponent_OnUpdate;

        // get the username for this player from the PlayerComponent table
        PlayerComponent playerComp = (PlayerComponent)PlayerComponent.FilterByEntityId(EntityId);        
        Username = playerComp.Nickname;

        // get the last location for this player and set the initial 
        // position 
        MobileEntityComponent mobPos = (MobileEntityComponent)MobileEntityComponent.FilterByEntityId(EntityId);
        Vector3 playerPos = new Vector3(mobPos.Position.X, 0.0f, mobPos.Position.Z);
        Quaternion playerRot = new Quaternion(mobPos.Rotation.X, mobPos.Rotation.Y, mobPos.Rotation.Z, mobPos.Rotation.W);
        transform.position = new Vector3(playerPos.x, MathUtil.GetTerrainHeight(playerPos), playerPos.z);
        transform.rotation = new Quaternion(mobPos.Rotation.X, mobPos.Rotation.Y, mobPos.Rotation.Z, mobPos.Rotation.W);
    }

    void DestroyCamera()
    {
        Transform cameraTransform = transform.Find("PlayerCameraRoot");
        if (cameraTransform != null)
        {
            // Objek "CameraController" ditemukan
            GameObject cameraObject = cameraTransform.gameObject;
            // Hancurkan objek "CameraController"
            Destroy(cameraObject);
        }
        else
        {
            // Objek "CameraController" tidak ditemukan
            Debug.LogError("Objek 'CameraController' tidak ditemukan!");
        }
    }
    private void MobileEntityComponent_OnUpdate(MobileEntityComponent oldObj, MobileEntityComponent obj, ReducerEvent callInfo)
    {
        // if the update was made to this object
        if (obj.EntityId == EntityId)
        {
            // update the DirectionVec in the PlayerMovementController component with the updated values
            var movementController = GetComponent<PlayerController>();
            movementController.movement =  new Vector3(obj.Direction.X, 0.0f, obj.Direction.Z);
            movementController.currentRotation = obj.Rotvalue;
            Quaternion objUnityRotation = obj.Rotation.ToUnityQuaternion(); // Convert StdbQuaternion to Quaternion
            //movementController.deltaRotation = new Quaternion(obj.Rotvalue.X, obj.Rotvalue.Y, obj.Rotvalue.Z, obj.Rotvalue.W);
            // if DirectionVec is {0,0,0} then we came to a stop so correct our position to match the server
            if (movementController.movement == Vector3.zero)
            {
                Vector3 playerPos = new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z);
                transform.position = new Vector3(playerPos.x, MathUtil.GetTerrainHeight(playerPos), playerPos.z);
            }

            if (movementController.currentRotation == 0.0f && transform.rotation != objUnityRotation)
            {
                Quaternion playerRot = new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W);
                transform.rotation = new Quaternion(playerRot.x, playerRot.y, playerRot.z, playerRot.w);
            }
        }
    }

    public void Interact()
    {
        GetComponentInChildren<AnimasiController>().intreact();
        Debug.Log("Remote Player Interact");
    }
}
