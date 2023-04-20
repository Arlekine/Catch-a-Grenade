using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Destractable
{
    public Action Dead;
    public UnityEvent Exploded;

    public enum EnemyPosition
    {
        Stand,
        OnKnee,
        Driving
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private List<Rigidbody> _rigidbodiesToForce = new List<Rigidbody>();
    [SerializeField] private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    [SerializeField] private EnemyPosition _enemyPosition;

    public bool IsDead { get; private set; }

    private void Start()
    {
        _animator.Play(_enemyPosition.ToString());
    }

    public override List<Rigidbody> Destract()
    {
        ActivateRagdoll();
        if (IsDead == false)
        {
            IsDead = true;
            Dead?.Invoke();
        }

        Exploded?.Invoke();
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