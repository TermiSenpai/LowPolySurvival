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
    public bool sprintToggle = false;

    private IMovable _movable;
    private IJumpable _jumpable;
    private IRotatable _rotatable;
    private PlayerMovement _playerMovement;

    private Vector2 moveInput;
    private bool sprintInputActive;

    private void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        var cameraTransform = Camera.main.transform;

        _playerMovement = new PlayerMovement(rb, moveSpeed, sprintSpeed, jumpForce);
        _movable = _playerMovement;
        _jumpable = _playerMovement;
        _rotatable = new PlayerRotation(transform, cameraTransform, lookSensitivity, maxLookAngle);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Store move input to be used in FixedUpdate
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _rotatable.Rotate(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _jumpable.Jump();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (sprintToggle)
        {
            if (context.performed)
            {
                // Toggle sprint state
                _playerMovement.SetSprinting(!_playerMovement.IsSprinting);
            }
        }
        else
        {
            // Set sprint state based on input
            sprintInputActive = context.ReadValueAsButton();
            _playerMovement.SetSprinting(sprintInputActive);
        }
    }

    private void FixedUpdate()
    {
        // Apply movement based on the current input and sprint state
        _movable.Move(moveInput);
        _playerMovement.ApplyGravity();
    }
}