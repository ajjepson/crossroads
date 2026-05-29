using UnityEngine;

public class ThridPersonCamera : MonoBehaviour
{

    public Transform target;

    private bool cursorLocked = true;

    public float mouseSensitivity = 200f;
    public float distance = 6f;
    public float height = 2f;

    public float minY = -30f;
    public float maxY = 60f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        // press esc to toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // rotate camera around player
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        Vector3 offset = new Vector3(0, height, -distance);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }
}
