using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Destractable
{
    public enum EnemyPosition
    {
        Stand,
        OnKnee
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private List<Rigidbody> _rigidbodiesToForce = new List<Rigidbody>();
    [SerializeField] private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    [SerializeField] private EnemyPosition _enemyPosition;

    private void Start()
    {
        _animator.Play(_enemyPosition.ToString());
    }

    public override List<Rigidbody> Destract()
    {
        ActivateRagdoll();
        return _rigidbodiesToForce;
    }

    [EditorButton]
    public void ActivateRagdoll()
    {
        _animator.enabled = false;
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = false;
        }
    }
}