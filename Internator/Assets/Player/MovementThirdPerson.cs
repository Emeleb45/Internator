using UnityEngine;
using Unity.Cinemachine;

public class CharacterMovementThirdPerson : MonoBehaviour
{
    private CharacterController characterController;
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    public float speed = 3.0f;
    public bool isRunning = false;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public CinemachineCamera virtualCamera; 



    public Transform groundCheck; 
    public float groundCheckRadius = 0.001f; 
    public float animationGroundCheckRadius = 0.25f;
    public LayerMask groundLayer; 


    private float ySpeed = 0f;  
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
        // Update is only used for camera zoom now
    }

    public void MovePlayer(Vector3 inputDirection, bool jumpPressed)
    {
        if (characterController.enabled == false)
        {
            return;
        }

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
            ySpeed = jumpSpeed; 
        }

        Vector3 forward = virtualCamera.transform.forward;
        Vector3 right = virtualCamera.transform.right;

        forward.y = 0; 
        right.y = 0;

        forward.Normalize();
        right.Normalize();


        moveDirection = forward * inputDirection.z + right * inputDirection.x;


        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        if (moveDirection.magnitude > 0.1f)
        {

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }


        characterController.Move(moveDirection * speed * Time.deltaTime);


        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude * speed);
            animator.SetBool("IsGrounded", IsGroundedForAnimation());
        }


        characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {

        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private bool IsGroundedForAnimation()
    {

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
}
