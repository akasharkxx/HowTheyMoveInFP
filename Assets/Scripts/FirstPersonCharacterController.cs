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
    public float crouchScaleFactor = 0.25f;
    
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
            if(transform.localScale.y >= crouchScaleFactor)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchScaleFactor, transform.localScale.z);
            }
        }
        else
        {
            if(transform.localScale.y < 1.0f)
            {
                transform.localScale += new Vector3(0f, 0.25f * Time.deltaTime, 0f);
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
