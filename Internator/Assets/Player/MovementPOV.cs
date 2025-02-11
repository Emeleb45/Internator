using UnityEngine;
using Unity.Cinemachine;

public class CharacterMovementPOV : MonoBehaviour
{
    private CharacterController characterController;
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    public float speed = 3.0f;
    public bool isRunning = false;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public Transform cameraTransform; // Reference to the camera
    public float mouseSensitivity = 2.0f;
    private float verticalRotation = 0f;

    public Transform groundCheck; // Transform for ground check
    public float groundCheckRadius = 0.001f; // Radius for ground check sphere
    public float animationGroundCheckRadius = 0.25f;
    public LayerMask groundLayer; // Layer to consider as ground


    private float ySpeed = 0f;  // For jump and gravity
    private Vector3 moveDirection = Vector3.zero;
    public Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
    }

    public void MovePlayer(Vector3 inputDirection, bool jumpPressed)
    {
        if (characterController.enabled == false)
        {
            return;
        }
        // Gravity handling and jumping
        if (IsGrounded())
        {
            ySpeed = -gravity * Time.deltaTime;
        }
        else
        {
            ySpeed -= gravity * Time.deltaTime;
        }
        moveDirection = new Vector3(inputDirection.x, 0, inputDirection.z);
        if (jumpPressed && IsGroundedForAnimation())
        {
            ySpeed = jumpSpeed; // Set jump speed when the jump button is pressed
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0; // Remove any tilt from the camera
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Combine input movement
        moveDirection = forward * inputDirection.z + right * inputDirection.x;

        // Normalize the movement vector to prevent faster diagonal movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        // Apply movement
        characterController.Move(moveDirection * speed * Time.deltaTime);

        // Send speed to the animation controller
        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude * speed);
            animator.SetBool("IsGrounded", IsGroundedForAnimation());
        }

        // Apply vertical movement (gravity/jump)
        characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        // Perform a sphere check at the groundCheck position
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private bool IsGroundedForAnimation()
    {
        // Perform a sphere check at the groundCheck position with a larger radius for animation
        return Physics.CheckSphere(groundCheck.position, animationGroundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, animationGroundCheckRadius);
        }
    }
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
