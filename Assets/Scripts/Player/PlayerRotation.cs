// File: IMovable.cs
using UnityEngine;

public class PlayerRotation : IRotatable
{
    private readonly Transform _playerTransform;
    private readonly Transform _cameraTransform;
    private readonly float _lookSensitivity;
    private readonly float _maxLookAngle;
    private float _currentXRotation;

    public PlayerRotation(Transform playerTransform, Transform cameraTransform, float lookSensitivity, float maxLookAngle)
    {
        _playerTransform = playerTransform;
        _cameraTransform = cameraTransform;
        _lookSensitivity = lookSensitivity;
        _maxLookAngle = maxLookAngle;
    }

    public void Rotate(Vector2 lookInput)
    {
        // Horizontal rotation (Y axis)
        float yRotation = lookInput.x * _lookSensitivity;
        _playerTransform.Rotate(Vector3.up * yRotation);

        // Vertical rotation (X axis)
        _currentXRotation -= lookInput.y * _lookSensitivity;
        _currentXRotation = Mathf.Clamp(_currentXRotation, -_maxLookAngle, _maxLookAngle);
        _cameraTransform.localRotation = Quaternion.Euler(_currentXRotation, 0f, 0f);
    }
}
