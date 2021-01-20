using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCharacterController : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float runSpeed = 2.5f;
    public float strafeSpeed = 1.0f;
    public float jumpHeight = 5.0f;
    public float gravityForce = 9.81f;
    public float groundDistance = 0.4f;
    
    public LayerMask groundLayer;

    public Transform groundCheck;

    private CharacterController controller;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        velocity = Vector3.zero;
    }

    private void Update()
    {
        SimpleMovement();
    }

    private void SimpleMovement()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        bool isRunPressed = Input.GetKey(KeyCode.LeftShift);
        bool isCrouchPressed = Input.GetKey(KeyCode.C);
        
        if(isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if(isCrouchPressed)
        {
            transform.localScale = Vector3.one * 0.25f;
        }
        else
        {
            transform.localScale += Vector3.one * 0.25f * Time.deltaTime;
            if (transform.localScale.x > 1.0f || transform.localScale.y > 1f || transform.localScale.z > 1f)
            {
                transform.localScale = Vector3.one;
            }
        }

        float forwardSpeed = verticalInput * (isRunPressed ? runSpeed : moveSpeed);
        float sideSpeed = horizontalInput * strafeSpeed;

        Vector3 moveDirection = transform.forward * forwardSpeed + transform.right * sideSpeed;

        controller.Move(moveDirection * Time.deltaTime);

        bool isJumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (isGrounded && isJumpPressed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        }

        velocity.y += gravityForce * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
