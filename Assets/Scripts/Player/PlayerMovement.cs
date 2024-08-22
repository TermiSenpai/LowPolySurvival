using UnityEngine;

public class PlayerMovement : IMovable, IJumpable
{
    private readonly Rigidbody _rigidbody;
    private readonly float _moveSpeed;
    private readonly float _sprintSpeed;
    private readonly float _jumpForce;
    private bool _isGrounded;
    private bool _isSprinting;
    private readonly float _gravityMultiplier = 2.0f; // Factor extra para incrementar la gravedad en la caída
    private readonly float _fallMultiplier = 2.5f; // Factor para incrementar la caída

    public PlayerMovement(Rigidbody rigidbody, float moveSpeed, float sprintSpeed, float jumpForce)
    {
        _rigidbody = rigidbody;
        _moveSpeed = moveSpeed;
        _sprintSpeed = sprintSpeed;
        _jumpForce = jumpForce;
    }

    public void Move(Vector2 moveInput)
    {
        _isGrounded = Physics.Raycast(_rigidbody.position, Vector3.down, 1.1f);
        float currentSpeed = _isSprinting ? _sprintSpeed : _moveSpeed;

        Vector3 moveDirection = _rigidbody.transform.forward * moveInput.y + _rigidbody.transform.right * moveInput.x;
        _rigidbody.MovePosition(_rigidbody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    public bool IsSprinting => _isSprinting;

    public void SetSprinting(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    public void ApplyGravity()
    {
        // Aplica gravedad adicional cuando el personaje está cayendo
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Aplica menos gravedad si se está ascendiendo (para mantener un salto más controlado)
        else if (_rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (_gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}
