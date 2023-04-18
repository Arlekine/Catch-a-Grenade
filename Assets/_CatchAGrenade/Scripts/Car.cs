using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private PathCreator _path;

    [Space] 
    [SerializeField] private Transform[] _wheels;

    private float _currentDistance;

    private void Update()
    {
        _currentDistance += _speed * Time.deltaTime;
        transform.position = _path.path.GetPointAtDistance(_currentDistance);
        transform.forward = _path.path.GetDirectionAtDistance(_currentDistance);

        foreach (var wheel in _wheels)
        {
            wheel.localEulerAngles += Vector3.right * 360f * Time.deltaTime;
        }
    }
}
