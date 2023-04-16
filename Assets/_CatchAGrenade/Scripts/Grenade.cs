using UnityEngine;

public class Grenade : MonoBehaviour
{
    private float _speed;
    private GrenadeTrajectory _grenadeTrajectory;
    private bool _isMoving;
    private float _currentMoveDistance;

    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    public void MoveAlongTrajectory(GrenadeTrajectory grenadeTrajectory, float speed)
    {
        _speed = speed;
        _grenadeTrajectory = grenadeTrajectory;
        _isMoving = true;
        _currentMoveDistance = 0f;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        /*print(other.gameObject.name);
        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _grenadeTrajectory.GetDirectionAtDistance(_currentMoveDistance + 0.2f) * _speed;

        _isMoving = false;*/
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (_currentMoveDistance < _grenadeTrajectory.PathLength)
            {
                _currentMoveDistance += _speed * Time.deltaTime;
                transform.position = _grenadeTrajectory.GetPointAtDistance(_currentMoveDistance);
            }
            else
            {
                Blow();
            }
        }
    }
}