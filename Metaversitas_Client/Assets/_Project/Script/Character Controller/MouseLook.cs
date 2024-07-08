using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Vector2 look;
    public static MouseLook Instance;

    private float xRotation = 0f;

    void Start()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.y * mouseSensitivity * Time.deltaTime;

        // Mengubah rotasi sumbu Y dari objek yang dikontrol
        //playerBody.Rotate(Vector3.up * mouseX);

        // Menghitung rotasi pada sumbu X (vertikal)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Mengatur rotasi lokal objek yang mengontrol kamera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

}
