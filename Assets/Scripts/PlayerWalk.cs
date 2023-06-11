using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    
    public CharacterController controller;

    public float playerWalkSpeed = 10f;
    public float playerSprintSpeed = 17f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask wallMask;

    Vector3 velocity;
    bool isGrounded;
    bool isWall;
    bool canJumpWall;
    float freeFall;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isWall = Physics.CheckSphere(groundCheck.position, groundDistance, wallMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJumpWall = true;
            freeFall = transform.position.y;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetKey(sprintKey))
        {
            controller.Move(move * playerSprintSpeed * Time.deltaTime);
        } else 
        {
            controller.Move(move * playerWalkSpeed * Time.deltaTime);
        }

        if(Input.GetKey(jumpKey) && (isGrounded || isWall) && canJumpWall)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if(isWall)
            {
                canJumpWall = false;
            }
        }

        velocity.y  += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

}
