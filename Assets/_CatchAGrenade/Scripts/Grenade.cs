using System;
using DG.Tweening;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private Collider _collider;
    [SerializeField] private LayerMask _obstaclesLayer;

    [Header("Explosion")] 
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    private float _speed;
    private bool _isMoving;
    private Vector3 _currentDirection;
    private float _currentFlightTime;
    private int _reflections;
    private DirectionAfterHit[] _directionAfterHits;
    private float _nextRaycastPossible;
    private bool IsRaycastPosiible => Time.time > _nextRaycastPossible;

    public void MoveAlongTrajectory(Vector3 startDirection, DirectionAfterHit[] directionAfterHits, float speed)
    {
        _speed = speed;
        _isMoving = true;
        _currentFlightTime = 0f;
        _reflections = 0;
        _currentDirection = startDirection;
        _directionAfterHits = directionAfterHits;
    }

    public void Stop()
    {
        _isMoving = false;
    }

    private void Blow()
    {
        gameObject.SetActive(false);
        _isMoving = false;
        Instantiate(_explosion, transform.position, Quaternion.identity);
        new Explosion(transform.position, _explosionRadius, _explosionForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _currentDirection);
    }

    private float GetVerticalSpeed(float flightTime)
    {
        return flightTime * Physics.gravity.y;
    }

    private void Update()
    {
        if (_isMoving)
        {
            _currentFlightTime += Time.deltaTime;

            var forwardVector = _currentDirection * _speed * Time.deltaTime;
            var gravityVector = Vector3.up * GetVerticalSpeed(_currentFlightTime) * Time.deltaTime;

            var currentPosition = transform.position;
            currentPosition += forwardVector;
            currentPosition += gravityVector;

            var stepDirection = currentPosition - transform.position;

            if (IsRaycastPosiible)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(transform.position, stepDirection.normalized, out raycastHit,
                        stepDirection.magnitude, _obstaclesLayer))
                {
                    currentPosition = raycastHit.point;

                    if (Vector3.Distance(raycastHit.point, _directionAfterHits[_reflections].HitPoint) > 0.2f)
                    {
                        _currentDirection = -stepDirection +
                                            2 * (Vector3.Dot(stepDirection, raycastHit.normal) * raycastHit.normal);
                        _currentDirection = -_currentDirection;
                        _currentFlightTime = 0;
                    }
                    else
                    {
                        _currentFlightTime = 0;
                        _currentDirection = _directionAfterHits[_reflections].AfterHitDirection;
                    }

                    _nextRaycastPossible = Time.time + 0.05f;
                    _reflections++;

                    if (raycastHit.collider.gameObject.GetComponent<EnemyBodyPart>() != null || _reflections >= 3)
                    {
                        Blow();
                    }
                }
            }

            transform.position = currentPosition;
            transform.eulerAngles += (Vector3.up * 600f * Time.deltaTime);
        }
    }
}

public struct DirectionAfterHit
{
    public Vector3 HitPoint;
    public Vector3 AfterHitDirection;
}