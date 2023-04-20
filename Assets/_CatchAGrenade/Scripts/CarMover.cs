using System;
using PathCreation;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    public Action PathEnded;

    [SerializeField] private float _speed;
    [SerializeField] private PathCreator _path;
    [SerializeField] private AudioSource _audioSource;

    [Space] 
    [SerializeField] private Transform[] _wheels;

    private float _currentDistance = 0;
    private bool _canMove = false;

    private void Start()
    {
        transform.position = _path.path.GetPointAtDistance(_currentDistance, EndOfPathInstruction.Stop);
        transform.forward = _path.path.GetDirectionAtDistance(_currentDistance, EndOfPathInstruction.Stop);
    }

    public void Activate()
    {
        _canMove = true;
    }

    private void Update()
    {
        if (_canMove == false)
            return;

        _currentDistance += _speed * Time.deltaTime;
        transform.position = _path.path.GetPointAtDistance(_currentDistance, EndOfPathInstruction.Stop);
        transform.forward = _path.path.GetDirectionAtDistance(_currentDistance, EndOfPathInstruction.Stop);

        foreach (var wheel in _wheels)
        {
            wheel.localEulerAngles += Vector3.right * 360f * Time.deltaTime;
        }

        if (_currentDistance >= _path.path.length)
        {
            enabled = false;
            _audioSource.Stop();
            PathEnded?.Invoke();
        }
    }
}