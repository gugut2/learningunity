using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private MovementInput playerInput;

    [SerializeField] private Transform cameraController;

    private bool isMoving = false;

    private Vector3 direction;
    private Vector3 currentDirection;

    [Header("Dash Variables")]
    private bool canMove = true;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 30f;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetMoveInput();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        rigidbody.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
        if(playerInput.MoveInput.x == 0 && playerInput.MoveInput.y == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void GetMoveInput() 
    {
        //Camera direction
        Vector3 cameraForward = cameraController.forward;
        Vector3 cameraRight = cameraController.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        //Camera relative direction
        Vector3 forwardRelative = playerInput.MoveInput.y * cameraForward;
        Vector3 rightRelative = playerInput.MoveInput.x * cameraRight;

        Vector3 movementDirection = forwardRelative + rightRelative;

        //Can only switch directions if not dashing
        if(canMove == true)
        {
            direction = new Vector3(movementDirection.x, 0, movementDirection.z);
            currentDirection = direction;
        }
    }

}
