using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public static Look instance;

    public bool updatingRotation;

    public float mouseSen = 100f;
    public Transform player;
    float xRotation = 0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Låser vår mus till skärmen, och så den inte syns. 
        Cursor.visible = false;
    }

    void Update()
    {
        if (updatingRotation) return;

        //Hämtar vår axis från input manager under project settings
        float mouseX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSen * Time.deltaTime;
        xRotation -= mouseY;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Vi använder rotate för att vi ska kunna stoppa rotationen från att gå förlångt / snurra ett helt varv
        player.Rotate(Vector3.up * mouseX);
    }
}
