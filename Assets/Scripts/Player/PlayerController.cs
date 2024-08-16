using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float lookSensitivity = 2f;
    public float maxLookAngle = 80f;
    public bool sprintToggle = false;  // True: toggle mode, False: hold to sprint

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Transform cameraTransform;
    private float currentXRotation = 0f;
    private bool isGrounded;
    private bool isSprinting;
    private bool sprintInputActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        // Lock the cursor for better control
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (sprintToggle)
        {
            // Toggle mode
            if (context.performed)
            {
                isSprinting = !isSprinting;
            }
        }
        else
        {
            // Hold mode
            sprintInputActive = context.ReadValueAsButton();
            isSprinting = sprintInputActive;
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Determine movement speed based on sprint status
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Move the player in the local Z axis
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        // Rotate the player horizontally (Y axis)
        float yRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * yRotation);

        // Rotate the camera vertically (X axis)
        currentXRotation -= lookInput.y * lookSensitivity;
        currentXRotation = Mathf.Clamp(currentXRotation, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
    }
}
