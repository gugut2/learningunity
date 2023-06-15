using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerInput;
    private CharacterController playerController;
    private ParticleSystem particleController;
    private Vector3 direction;
    private Vector3 currentDirection;

    [SerializeField] Transform cameraController;

    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;

    [SerializeField] private float speed = 7f;
    private float initialSpeed = 7f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    private bool isMoving = false;

    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 1f;
    private Vector3 dashDirection;
    private bool canDash = true;
    private bool canMove = true;

    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 2f;
    private float velocity;

    [SerializeField] private float jumpPower;
    private float jumpCooldown = 0.4f;
    private bool canJump = true;

    private int numberOfJumps;
    [SerializeField] private int maxNumberOfJumps;
    
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        particleController = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (isGrounded() && velocity < 0f)
        {
            velocity = -1f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        currentDirection.y = velocity;
    }

    private void ApplyRotation()
    {
        if (playerInput.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        playerController.Move(currentDirection * speed * Time.fixedDeltaTime);
        if(playerInput.x == 0 && playerInput.y == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    public void Move(InputAction.CallbackContext context) 
    {
        playerInput = context.ReadValue<Vector2>();   

        //Camera direction
        Vector3 cameraForward = cameraController.forward;
        Vector3 cameraRight = cameraController.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        //Camera relative direction
        Vector3 forwardRelative = playerInput.y * cameraForward;
        Vector3 rightRelative = playerInput.x * cameraRight;

        Vector3 movementDirection = forwardRelative + rightRelative;

        if(canMove == true)
        {
            direction = new Vector3(movementDirection.x, 0, movementDirection.z);
            currentDirection = direction;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        if(!isGrounded() && numberOfJumps >= maxNumberOfJumps) return;
        if(canJump == false) return;
        if(numberOfJumps == 0) StartCoroutine(WaitForLanding());
        
        canJump = false;
        StartCoroutine(JumpCooldown());
        
        numberOfJumps++;

        velocity += jumpPower;
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !isGrounded());
        yield return new WaitUntil(isGrounded);

        numberOfJumps = 0;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(isGrounded())
        {
            speed = initialSpeed * sprintMultiplier;
            particleController.Play();
        }

        if (context.canceled || !isGrounded())
        {
            speed = initialSpeed;
            particleController.Stop();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        if(canDash == true && isGrounded())
        {
            canDash = false;
            canJump = false;
            canMove = false;
            if(context.started)
            {
                if(isMoving == true){
                    dashDirection = transform.forward * dashSpeed;    
                }
                else{
                    dashDirection = -transform.forward * dashSpeed; 
                }

                currentDirection = dashDirection;
            }
            StartCoroutine(JumpCooldown());
            StartCoroutine(StopDashing());
            StartCoroutine(DashCooldown());  
        }

    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        canMove = true;
        currentDirection = direction;
        speed = initialSpeed;
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private bool isGrounded() => playerController.isGrounded;
}
