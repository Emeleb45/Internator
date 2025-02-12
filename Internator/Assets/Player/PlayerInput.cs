using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterMovementPOV movementScript;


    private Vector3 inputDirection = Vector3.zero;
    private bool isRunning = false;
    private bool jumpPressed = false;
    private bool toggleRunPressed = false;


    // ###### CONTROLS LIST
    public KeyCode runKey = KeyCode.LeftControl;



    // ###### END CONTROLS LIST

    void Start()
    {

        movementScript = GetComponent<CharacterMovementPOV>();

        if (movementScript == null)
        {
            Debug.LogError("ExampleClass script not found on the GameObject!");
        }
    }

    void Update()
    {
        // ###### Gather input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical);



        isRunning = Input.GetKey(runKey);

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        // ###### Pass movement input to the movement script
        if (movementScript != null)
        {
            movementScript.isRunning = isRunning;
            movementScript.speed = isRunning ? movementScript.runSpeed : movementScript.walkSpeed;

            movementScript.MovePlayer(inputDirection, jumpPressed);

            if (jumpPressed)
            {
                jumpPressed = false;
            }
        }

    }

}
