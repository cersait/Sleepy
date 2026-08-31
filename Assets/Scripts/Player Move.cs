using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = walkSpeed;
    public float gravity = -9f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    bool isGrounded;

    private const float walkSpeed = 10f;
    private float runSpeed = 15f;
    public float movementSpeedMultiplier = 1f;
    [HideInInspector] public StaminaController staminaController;

    public float Height
    {
        get => controller.height;
        set => controller.height = value;
    }
    public Transform cameraTransform;
    public event Action OnBeforeMove;

    private void Start()
    {
        staminaController = GetComponent<StaminaController>();

        if (staminaController == null)
        {
            Debug.LogError("Playemove no staminacontroller found");
        }

        moveSpeed = walkSpeed;
    }

    public void SetMovementSpeedMultiplier(float multiplier)
    {
        movementSpeedMultiplier = multiplier;
    }

    public void SetRunSpeed(float speed)
    {
        runSpeed = speed;
    }
    void Update()
    {
        OnBeforeMove?.Invoke();


        //check ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //minus två så den inte regristrerar innan vi nått marken
        }

        //check movement
        float x = Input.GetAxis("Horizontal"); //Gå med WASD
        float z = Input.GetAxis("Vertical"); // -.- 
        Vector3 move = transform.right * x + transform.forward * z; //Rör sig i den riktningen som player också tittar i
        controller.Move(move * moveSpeed * movementSpeedMultiplier  * Time.deltaTime); //Ref till vår charactercontroller som driver vår player + låter oss röra på oss
        
        //check gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //check sprint
        if (staminaController == null)
        {
            return;
        }

        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);

        if (wantsToSprint && staminaController.playerStamina > 0)
        {
            moveSpeed = runSpeed;

            staminaController.weAreSprinting = true;
            staminaController.Sprinting();
        }
        else
        {
            moveSpeed = walkSpeed;

            staminaController.weAreSprinting = false;
        }
    }
}
