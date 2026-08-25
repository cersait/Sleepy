using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = walkSpeed;
    public float gravity = -9f;
    public float jumpHeight = 3f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    bool isGrounded;

    private const float walkSpeed = 10f;
    private const float runSpeed = 15f;
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //minus två så den inte regristrerar innan vi nått marken
        }
        float x = Input.GetAxis("Horizontal"); //Gå med WASD
        float z = Input.GetAxis("Vertical"); // -.- 
        Vector3 move = transform.right * x + transform.forward * z; //Rör sig i den riktningen som player också tittar i
        controller.Move(move * moveSpeed * Time.deltaTime); //Ref till vår charactercontroller som driver vår player + låter oss röra på oss
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }
}
