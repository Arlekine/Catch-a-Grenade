using System;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Action Exploded;

    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Collider _collider;
    [SerializeField] private LayerMask _obstaclesLayer;

    [Header("Explosion")] 
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    [Header("Sounds")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _throw;
    [SerializeField] private AudioClip _explode;
    [SerializeField] private AudioClip _hit;

    private Zone _zone;
    private float _speed;
    private bool _isMoving;
    private Vector3 _currentDirection;
    private float _currentFlightTime;
    private int _reflections;
    private DirectionAfterHit[] _directionAfterHits;
    private float _nextRaycastPossible;
    private bool IsRaycastPosiible => Time.time > _nextRaycastPossible;

    private GameData _data;

    private float _inColliderTime;
    private bool _isInCollider;

    public void SetZone(Zone zone)
    {
        _zone = zone;
    }

    public void SetGameData(GameData data)
    {
        _data = data;
    }

    public void MoveAlongTrajectory(Vector3 startDirection, DirectionAfterHit[] directionAfterHits, float speed)
    {
        if (_data.HapticOn)
            MMVibrationManager.Haptic(HapticTypes.LightImpact);

        PlaySound(_throw);

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
        _mesh.enabled = (false);
        _isMoving = false;
        var explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
        explosion.transform.parent = transform.parent;
        Exploded?.Invoke();

        if (_data.HapticOn)
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

        new Explosion(transform.position, _explosionRadius, _explosionForce);

        PlaySound(_explode);
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
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
                if (Physics.Raycast(transform.position, stepDirection.normalized, out raycastHit, stepDirection.magnitude * 3, _obstaclesLayer))
                {
                    currentPosition = raycastHit.point;

                    if (Vector3.Distance(raycastHit.point, _directionAfterHits[_reflections].HitPoint) > 0.3f)
                    {
                        _currentDirection = -stepDirection +
                                            2 * (Vector3.Dot(stepDirection, raycastHit.normal) * raycastHit.normal);
                        _currentDirection = -_currentDirection * 3;
                        _currentFlightTime = 0;
                    }
                    else
                    {
                        _currentFlightTime = 0;
                        _currentDirection = _directionAfterHits[_reflections].AfterHitDirection;
                    }

                    _nextRaycastPossible = Time.time + 0.1f;
                    _reflections++;

                    if (raycastHit.collider.gameObject.GetComponent<EnemyBodyPart>() != null || _reflections >= 3)
                    {
                        Blow();
                        return;
                    }
                    else
                    {
                        PlaySound(_hit);
                    }
                }
            }

            var isInCollider = false;
            var colliders = Physics.OverlapSphere(currentPosition, 0.3f);

            foreach (var col in colliders)
            {
                if (col.bounds.Contains(transform.position))
                {
                    isInCollider = true;

                    break;
                }
            }

            if (isInCollider)
            {
                if (_isInCollider == false)
                {
                    _isInCollider = true;
                    _inColliderTime = Time.time;
                }
            }
            else
            {
                _isInCollider = false;
                _inColliderTime = Time.time;
            }

            if (Time.time - _inColliderTime > 0.05f)
            {
                _currentDirection = -_currentDirection;

                _inColliderTime = Time.time;
                _reflections++;
                _currentFlightTime = 0;

                _nextRaycastPossible = Time.time + 0.075f;

                if (_reflections >= 3)
                {
                    Blow();
                    return;
                }
            }

            if (_zone.IsInZone(currentPosition) == false)
            {
                Blow();
                return;
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

[Serializable]
public class Zone
{
    [SerializeField] private float _yMax;
    [SerializeField] private float _radius;
    [SerializeField] private Vector3 _center;

    public Vector3 Center => _center;
    public float Radius => _radius;

    public bool IsInZone(Vector3 point)
    {
        var yDistance = Mathf.Abs(point.y - _center.y);
        point.y = _center.y;
        var planeDistance = Vector3.Distance(point, _center);

        if (planeDistance >= _radius || yDistance >= _yMax)
            return false;
        else
            return true;
    }
}