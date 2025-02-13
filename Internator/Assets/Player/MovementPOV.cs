using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CharacterMovementPOV : MonoBehaviour
{
    private CharacterController characterController;
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    public float speed = 3.0f;
    public bool isRunning = false;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public bool MovementLocked = false;
    public bool CameraLocked = false;

    public Transform cameraTransform;
    public float mouseSensitivity = 2.0f;
    private float verticalRotation = 0f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.001f;
    public float animationGroundCheckRadius = 0.25f;
    public LayerMask groundLayer;


    private float ySpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;
    public Animator animator;

    public float Points;
    public GameObject EndScreen;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public GameObject EndButton;
    public GameObject Tips;
    public bool TipsOn = false;
    private Quaternion targetRotation = Quaternion.Euler(25, 0, 0);
    public float smooth = 1.0f;
    public bool Speaking = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!CameraLocked)
        {
            HandleMouseLook();
        }
        if (TipsOn)
        {
            cameraTransform.localRotation = Quaternion.RotateTowards(cameraTransform.localRotation, targetRotation, smooth * Time.deltaTime * 100);
        }

    }

    public void MovePlayer(Vector3 inputDirection, bool jumpPressed)
    {
        if (characterController.enabled == false)
        {
            return;
        }
        if (MovementLocked && !TipsOn)
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
        if (MovementLocked)
        {
            characterController.Move(Vector3.up * ySpeed * Time.deltaTime);
            return;
        }
        moveDirection = new Vector3(inputDirection.x, 0, inputDirection.z);
        if (jumpPressed && IsGroundedForAnimation())
        {
            ySpeed = jumpSpeed;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();


        moveDirection = forward * inputDirection.z + right * inputDirection.x;


        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
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
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;


        transform.Rotate(Vector3.up * mouseX);


        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
    public void UpdatePoints(float amnt)
    {
        Points += amnt;
        if (Points >= 100)
        {
            // WIN
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MovementLocked = true;
            CameraLocked = true;
            EndScreen.SetActive(true);
            WinScreen.SetActive(true);
        }
        else if (Points <= -100)
        {
            // LOSE
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MovementLocked = true;
            CameraLocked = true;
            EndScreen.SetActive(true);
            LoseScreen.SetActive(true);
        }
    }
    public void ToggleTips()
    {
        if (TipsOn)
        {
            Tips.SetActive(false);
            TipsOn = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MovementLocked = false;
            CameraLocked = false;
        }
        else if (!TipsOn && !Speaking)
        {
            Tips.SetActive(true);
            TipsOn = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MovementLocked = true;
            CameraLocked = true;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
