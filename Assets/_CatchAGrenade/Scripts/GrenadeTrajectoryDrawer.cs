using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrajectoryDrawer : MonoBehaviour
{
    [SerializeField] private GrenadeThrower _grenadeThrower;
    [SerializeField] private GameObject _dotLinePrefab;
    [SerializeField] private float _firstScale;
    [SerializeField] private float _lastScale;
    [SerializeField] private int _dotsInLine;
    [SerializeField] private LayerMask _obstaclesLayer;

    [Header("Gizmo Line")] 
    [SerializeField] private int _gizmoAccuracy;
    [SerializeField] private int _gizmoMaxLength;

    private List<GameObject> _dots = new List<GameObject>();
    private List<Vector3[]> _reflections = new List<Vector3[]>();

    private void Update()
    {
        if (_dots.Count > 0)
        {
            PositionDotLine();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_grenadeThrower == null)
            return;

        //Render(_grenadeThrower.ThrowPoint, _grenadeThrower.StartSpeed, _gizmoMaxLength);

        foreach (var reflection in _reflections)
        {
            Gizmos.DrawLine(reflection[0], reflection[0] + reflection[1]);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(reflection[0], reflection[0] - reflection[2]);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(reflection[0], reflection[0] + reflection[3]);
        }
    }

    public GrenadeTrajectory GetTrajectory()
    {
        var points = new List<Vector3>();

        foreach (var dot in _dots)
        {
            if (dot.activeSelf == false)
                break;

            points.Add(dot.transform.position);
        }

        return new GrenadeTrajectory(points);
    }

    public void CreateLine()
    {
        if (_dots.Count > 0)
            HideLine();

        for (int i = 0; i < _dotsInLine; i++)
        {
            var newDot = Instantiate(_dotLinePrefab);
            _dots.Add(newDot);
        }
    }

    public void HideLine()
    {
        if (_dots.Count == 0 || _dots[0] == null)
            return;
        
        for (int i = 0; i < _dotsInLine; i++)
        {
            Destroy(_dots[i].gameObject);
        }

        _dots.Clear();
    }

    private void PositionDotLine()
    {
        float time = _gizmoMaxLength / _grenadeThrower.StartSpeed;
        float stepTime = time / _dotsInLine;

        Vector3 currentPosition = _grenadeThrower.Grenade.position;
        Vector3 previousPosition = currentPosition;

        Vector3 currentDirection = _grenadeThrower.ThrowPoint.forward;

        float currentTime = 0f;
        int reflections = 0;
        _reflections.Clear();

        for (int i = 0; i < _dots.Count; i++)
        {
            if (reflections >= 3)
            {
                _dots[i].gameObject.SetActive(false);
            }
            else
            {
                _dots[i].gameObject.SetActive(true);
                currentTime += stepTime;

                var forwardVector = currentDirection * _grenadeThrower.StartSpeed * stepTime;
                var gravityVector = Vector3.up * GetVerticalSpeed(currentTime) * stepTime;

                currentPosition += forwardVector;
                currentPosition += gravityVector;

                var stepDirection = currentPosition - previousPosition;

                RaycastHit raycastHit;
                if (Physics.Raycast(previousPosition, stepDirection.normalized, out raycastHit, stepDirection.magnitude,
                        _obstaclesLayer))
                {
                    var isNormalUp = raycastHit.normal.x == 0 && Math.Abs(raycastHit.normal.y - 1) < float.Epsilon &&
                                     raycastHit.normal.z == 0;

                    currentPosition = raycastHit.point;
                    currentDirection = -stepDirection +
                                       2 * (Vector3.Dot(stepDirection, raycastHit.normal) *
                                            raycastHit.normal); //Vector3.Reflect(stepDirection, raycastHit.normal);
                    currentDirection = -currentDirection;
                    currentTime = 0;

                    _reflections.Add(new Vector3[]
                        { raycastHit.point, raycastHit.normal, stepDirection, -currentDirection });

                    reflections++;
                }

                _dots[i].transform.position = currentPosition;
                _dots[i].transform.localScale = Vector3.one * Mathf.LerpAngle(_firstScale, _lastScale, (float)i / (float)_dots.Count);

                previousPosition = currentPosition;
            }
        }
    }

    private float GetVerticalSpeed(float flightTime)
    {
        return flightTime * Physics.gravity.y;
    }

    private void Render(Transform startPoint, float speed, float maxLength)
    {
        float time = maxLength / speed;
        float stepTime = time / _gizmoAccuracy;

        Vector3 currentPosition = startPoint.position;
        Vector3 previousPosition = currentPosition;

        float currentTime = 0f;

        for (int i = 0; i < _gizmoAccuracy; i++)
        {
            currentTime += stepTime;
            var forwardVector = startPoint.forward * speed * stepTime;
            var gravityVector = Vector3.up * GetVerticalSpeed(currentTime) * stepTime;

            currentPosition += forwardVector;
            currentPosition += gravityVector;

            Gizmos.DrawLine(previousPosition, currentPosition);

            previousPosition = currentPosition;
        }
    }
}