using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class ChaControl2 : MonoBehaviour
{
    //archer character
    private float playerSpeed = 9f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    //testing dash
    private bool canDash = false;
    public Image dashImage;
    public float dashLength = 3f;
    public float coolDownNumber = 4f;
    public bool canPlayerDash = true;
    //dash effects
    public float dashDistance = 16f;
    public float dashSpeed = 40f;
    private bool amIDashing = false;
    private Vector3 dashLoction;

    //camera
    public Transform cameraTransform;
    ThridPersonCamera cam;
    //camera
    //audio
    [SerializeField] private AudioClip archerWalkingAudio;
    private AudioSource audioArcherSource;

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
        audioArcherSource = GetComponent<AudioSource>();
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
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

            if (!audioArcherSource.isPlaying)
            {
                audioArcherSource.clip = archerWalkingAudio;
                audioArcherSource.Play();
            }
        }
        else
        {
            audioArcherSource.Stop();
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        if (controller.isGrounded && playerVelocity.y < 0 )
        {
            playerVelocity.y = -2f;
        }

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed);
        finalMove.y = playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

        //testing Dash
        if (Input.GetKey(KeyCode.LeftShift) && canPlayerDash && !amIDashing)
        {
            if (!canDash)
            {
                StartCoroutine(Dashing());
                StartDashing();
            }
        }
        if (amIDashing)
        {
            Vector3 dashDir = (dashLoction - transform.position).normalized;
            CollisionFlags flags = controller.Move(dashDir * dashSpeed * Time.deltaTime);
            if ((flags & CollisionFlags.CollidedSides) != 0)
            {
                amIDashing = false;
            }
            if (Vector3.Distance(transform.position, dashLoction) < 0.2f)
            {
                amIDashing = false;
            }
        }
    }
    private void StartDashing()
    {
        amIDashing = true;
        dashLoction = transform.position + transform.forward * dashDistance;
    }
    private IEnumerator Dashing()
    {
        canDash = true;
        canPlayerDash = false;
        //yield return new WaitForSeconds(sheildLength);

        float countUpTime = 0f;
        dashImage.fillAmount = 1f;

        while (countUpTime < dashLength)
        {
            countUpTime += Time.deltaTime;
            dashImage.fillAmount = 1f - (countUpTime / dashLength);
            yield return null;
        }
        //flip this to fill back up (replace countUpTime with countDownTime and dashLength with coolDownNumber)
        float countDownTime = 0f;
        dashImage.fillAmount = 0f;
        while (countDownTime < coolDownNumber)
        {
            countDownTime += Time.deltaTime;
            dashImage.fillAmount = (countDownTime / coolDownNumber);
            yield return null;
        }
        dashImage.fillAmount = 1f;
        canPlayerDash = true;
        canDash = false;
    }
}
