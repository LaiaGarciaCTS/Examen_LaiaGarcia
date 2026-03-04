using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour

{

public CharacterController player;
private Vector2 moveDirection;
private float movementSpeed = 5.0f;
private bool groundedPlayer;

[Header("Input Actions")]
public InputActionReference moveAction;
public InputActionReference jumpAction;


private void OnEnable()
{
    moveAction.action.Enable();
    jumpAction.action.Enable();
}

private void OnDisable()
{
    moveAction.action.Disable();
    jumpAction.action.Disable();
}


 void Update()
    {
        groundedPlayer = player.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

 // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
            transform.forward = move;

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);



        if (player.isGrounded)
        {
            print("CharacterController is grounded");
        }



         Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        // The speed on the x-z plane ignoring any speed
        float horizontalSpeed = horizontalVelocity.magnitude;
        // The speed from gravity or jumping
        float verticalSpeed  = controller.velocity.y;
        // The overall speed
        float overallSpeed = controller.velocity.magnitude;

    }
    void Start()
    {
        player = GetComponent<CharacterController>();
        controller = GetComponent<CharacterController>();
    }

    }



