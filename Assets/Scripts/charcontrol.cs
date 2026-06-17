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
    //remove sprint
    //public float sprintLength = 3f;
    //public float playertiredness = 4f;
    //public bool canPlayerSprint = true;
    //private bool canSprint = false;
<<<<<<< HEAD
    //public Image sprintImage;
=======
    public Image sprintImage;
>>>>>>> origin/main
    //remove sprint

    //new


    //audio
    [SerializeField] private AudioClip walkingAudio;
    private AudioSource audioWalkingSource;

    [Header("Input Actions")]
    public InputActionReference moveAction; 

    [Header("Mouse Aim")]
    public Camera playerCamera;
    public float waistHeight = 1f; // height above transform.position

    [Header("Knockback")]
    public float knockbackStrength = 12f;
    public float knockbackDuration = 0.25f;
    private bool isKnockedBack = false;
    private Vector3 knockbackVelocity;

    private void Awake()
    {
        cam = Camera.main.GetComponent<ThridPersonCamera>();
        controller = gameObject.AddComponent<CharacterController>();
        audioWalkingSource = GetComponent<AudioSource>();

        if (playerCamera == null)
            playerCamera = Camera.main;
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

        move = Vector3.ClampMagnitude(move, 1f);

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        if (!isKnockedBack)
        {
            Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
            controller.Move(finalMove * Time.deltaTime);
        }

        // ---- Mouse aim 
        Vector3 mouseAimPoint = GetMouseAimPoint();
        Vector3 aimDirection = mouseAimPoint - transform.position;
        aimDirection.y = 0f;

        if (aimDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Movement-based audio 
        if (move != Vector3.zero)
        {
            if (!audioWalkingSource.isPlaying)
            {
                audioWalkingSource.clip = walkingAudio;
                audioWalkingSource.Play();
            }
        }
        else
        {
            audioWalkingSource.Stop();
        }
        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        //Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        //controller.Move(finalMove * Time.deltaTime);
        //remove sprint
        /*
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
        */
    }
    private Vector3 GetMouseAimPoint()
    {
       
        Plane aimPlane = new Plane(Vector3.up, transform.position + Vector3.up * waistHeight);

        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (aimPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position + cameraTransform.forward;
    }
    //remove sprint
    /*
    private IEnumerator Sprint()
    {
        canSprint = true;
        canPlayerSprint = false;

        float countUpTime = 0f;
        sprintImage.fillAmount = 1f;

        while (countUpTime < sprintLength)
        {
            countUpTime += Time.deltaTime;
            sprintImage.fillAmount = 1f - (countUpTime / sprintLength);
            yield return null;
        }

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
    */
    public void ApplyKnockback(Vector3 sourcePosition)
    {
        Vector3 dir = (transform.position - sourcePosition).normalized;
        dir.y = 0f;

        knockbackVelocity = dir * knockbackStrength;

        StartCoroutine(KnockbackRoutine());
    }
    IEnumerator KnockbackRoutine()
    {
        isKnockedBack = true;

        float t = 0f;

        while (t < knockbackDuration)
        {
            t += Time.deltaTime;

            controller.Move(knockbackVelocity * Time.deltaTime);

            yield return null;
        }

        isKnockedBack = false;
    }
}
