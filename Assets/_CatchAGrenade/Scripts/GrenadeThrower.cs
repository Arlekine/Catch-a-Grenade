using System;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public Action GrenadeThrowed;
    public Action GrenadeExploded;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    [Header("")]
    [SerializeField] private Grenade _grenade;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private Transform _throwPointLeft;
    [SerializeField] private Transform _throwPointRight;

    private float _startSpeed;
    private Vector3 _standartPoint;
    private Quaternion _stadartRotation;
    private Transform _standartParent;

    public float StartSpeed => _startSpeed;
    public Transform ThrowPoint => _throwPoint;
    public Transform Grenade => _grenade.transform;

    private void Start()
    {
        _standartParent = _grenade.transform.parent;
        _standartPoint = _grenade.transform.localPosition;
        _stadartRotation = _grenade.transform.localRotation;
        _grenade.Exploded += () => GrenadeExploded?.Invoke();
    }

    public void SetData(GameData data)
    {
        _grenade.SetGameData(data);
    }

    public void SetControl(Vector2 control)
    {
        _throwPoint.forward = Vector3.Lerp(_throwPointLeft.forward, _throwPointRight.forward, 1f - control.x);
        _startSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, control.y);
    }

    public void ReturnGrenade()
    {
        _grenade.GetComponent<MeshRenderer>().enabled = (true);
        _grenade.transform.parent = _standartParent;
        _grenade.transform.localPosition = _standartPoint;
        _grenade.transform.localRotation = _stadartRotation;
        _grenade.Stop();
    }

    public void Throw(DirectionAfterHit[] directionAfterHits)
    {
        _grenade.transform.parent = transform.parent;
        _grenade.MoveAlongTrajectory(_throwPoint.forward, directionAfterHits, _startSpeed);
        GrenadeThrowed?.Invoke();
    }
}