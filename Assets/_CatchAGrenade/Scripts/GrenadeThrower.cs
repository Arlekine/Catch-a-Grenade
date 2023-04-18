using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    [Header("")]
    [SerializeField] private Grenade _grenade;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private Transform _throwPointLeft;
    [SerializeField] private Transform _throwPointRight;

    private float _startSpeed;

    public float StartSpeed => _startSpeed;
    public Transform ThrowPoint => _throwPoint;
    public Transform Grenade => _grenade.transform;

    private Vector3 _standartPoint;
    private Quaternion _stadartRotation;
    private Transform _standartParent;

    private void Start()
    {
        _standartParent = _grenade.transform.parent;
        _standartPoint = _grenade.transform.localPosition;
        _stadartRotation = _grenade.transform.localRotation;
    }

    public void SetControl(Vector2 control)
    {
        _throwPoint.forward = Vector3.Lerp(_throwPointLeft.forward, _throwPointRight.forward, 1f - control.x);
        _startSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, control.y);
    }

    public void ReturnGrenade()
    {
        _grenade.gameObject.SetActive(true);
        _grenade.transform.parent = _standartParent;
        _grenade.transform.localPosition = _standartPoint;
        _grenade.transform.localRotation = _stadartRotation;
        _grenade.Stop();
    }

    public void Throw(DirectionAfterHit[] directionAfterHits)
    {
        _grenade.transform.parent = null;
        _grenade.MoveAlongTrajectory(_throwPoint.forward, directionAfterHits, _startSpeed);
    }
}