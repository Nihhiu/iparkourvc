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
        //check if the player is touching an object of the type ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //check if the player is touching an object of the type wall
        isWall = Physics.CheckSphere(groundCheck.position, groundDistance, wallMask);

        //if the player is in the floor, then reset all changeable variables
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canJumpWall = true;
            freeFall = transform.position.y;
        }

        //movement coordenates
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //move to the right depending on X / move forward depending on Z
        Vector3 move = transform.right * x + transform.forward * z;

        //if sprintKey is being held, then player will move at sprinting speed, otherwise, at walking speed
        if(Input.GetKey(sprintKey))
        {
            controller.Move(move * playerSprintSpeed * Time.deltaTime);
        } else 
        {
            controller.Move(move * playerWalkSpeed * Time.deltaTime);
        }

        //if the player jumps, and the player is grounded or in a wall, and they can jump in a wall, it will jump
        if(Input.GetKey(jumpKey) && (isGrounded || isWall) && canJumpWall)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //after jumping one in a wall, they will no longer be able to, unless they touch the ground again to reset
            if(isWall)
            {
                canJumpWall = false;
            }
        }

        velocity.y  += gravity * Time.deltaTime;

        //will apply the jumping move
        controller.Move(velocity * Time.deltaTime);
    }

}
