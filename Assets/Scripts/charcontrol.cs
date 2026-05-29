using UnityEngine;
using UnityEngine.InputSystem;

public class charcontrol : MonoBehaviour
{
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    //camera
    public Transform cameraTransform;
    ThridPersonCamera cam;
    //camera

    [Header("Input Actions")]
    public InputActionReference moveAction; // expects Vector2

    private void Awake()
    {
        /*
        //camera
        cam = Camera.main.GetComponent<ThridPersonCamera>();
        //camera
        */
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        //camera
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 camFordward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camFordward.y = 0f;
        camRight.y = 0f;

        camFordward.Normalize();
        camRight.Normalize();

        Vector3 move = camFordward * input.y + camRight * input.x;
        move = Vector3.ClampMagnitude(move, 1f);
        //camera

        // Read input
        //added Vector2 input = moveAction.action.ReadValue<Vector2>();
        //added Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }
}
