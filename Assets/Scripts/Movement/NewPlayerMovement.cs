using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private MovementInput playerInput;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform cameraController;

    private Vector3 direction;
    private Vector3 forceDirection;
    private Vector3 currentDirection;

    [Header("Rotation")]
    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    [Header("Dash Variables")]
    private bool canMove = true;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 30f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded = true;
    [SerializeField] [Range(0.0f, 1.8f)] private float groundCheckRadius = 0.9f;
    [SerializeField] [Range(-0.95f, 1.05f)] private float groundCheckDistance = 0.05f;
    RaycastHit groundCheckHit = new RaycastHit();

    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 2f;
    private float velocity;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        GetMoveInput();
        ApplyRotation();
        PlayerMove();

        rigidbody.AddRelativeForce(forceDirection, ForceMode.Force);
    }
    
    private void ApplyRotation()
    {
        if (playerInput.MoveInput.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(forceDirection.x, forceDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    
    private void PlayerMove()
    {
        forceDirection = (new Vector3(direction.x * moveSpeed * rigidbody.mass, 
                                direction.y, 
                                direction.z * moveSpeed * rigidbody.mass));
    }

    public void GetMoveInput() 
    {
        direction = new Vector3(playerInput.MoveInput.x, 0, playerInput.MoveInput.y);
    }
}
