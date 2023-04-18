using UnityEngine;

public class CameraCenterRotation : MonoBehaviour
{
    [SerializeField] private float _leftRotation;
    [SerializeField] private float _rightRotation;

    private float _currentRotation = 0.5f;
    private float _targetRotation = 0.5f;

    public void Rotate(float rotation)
    {
        _targetRotation = 1f - rotation;
    }

    private void Update()
    {
        _currentRotation = Mathf.Lerp(_currentRotation, _targetRotation, 2f * Time.deltaTime);
        transform.eulerAngles = Vector3.up * Mathf.Lerp(_leftRotation, _rightRotation, _currentRotation);
    }
}