using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class charcontrol : MonoBehaviour
{
    //knight
    private float playerSpeed = 9f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    //camera
    public Transform cameraTransform;
    ThridPersonCamera cam;
    //camera
    //new
    public float sprintLength = 3f;
    public float playertiredness = 4f;
    public bool canPlayerSprint = true;
    private bool canSprint = false;
    public Image sprintImage;
    //new
    [Header("Input Actions")]
    public InputActionReference moveAction; // expects Vector2

    private void Awake()
    {
        cam = Camera.main.GetComponent<ThridPersonCamera>();
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
            //transform.forward = move;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && canPlayerSprint == true)
        {
            playerSpeed = 12f;
            if (!canSprint)
            {
                StartCoroutine(Sprint());
            }
        }
        else
        {
            playerSpeed = 9f;
        }
    }
    private IEnumerator Sprint()
    {
        canSprint = true;
        canPlayerSprint = false;
        //yield return new WaitForSeconds(sheildLength);

        float countUpTime = 0f;
        sprintImage.fillAmount = 1f;

        while (countUpTime < sprintLength)
        {
            countUpTime += Time.deltaTime;
            sprintImage.fillAmount = 1f - (countUpTime / sprintLength);
            yield return null;
        }
        //flip this to fill back up (replace countUpTime with countDownTime and sprintLength with playertiredness)
        float countDownTime = 0f;
        sprintImage.fillAmount = 0f;
        while (countDownTime < playertiredness)
        {
            countDownTime += Time.deltaTime;
            sprintImage.fillAmount = (countDownTime / playertiredness);
            yield return null;
        }
        sprintImage.fillAmount = 1f;
        canPlayerSprint = true;
        canSprint = false;
    }
}
