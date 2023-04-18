using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destractable
{
    [SerializeField] private ParticleSystem _explosionPrefab;
    [SerializeField] private MeshRenderer _model;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    public override List<Rigidbody> Destract()
    {
        GetComponent<Collider>().enabled = false;
        _model.enabled = false;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        StartCoroutine(ExplosionRoutine());

        return new List<Rigidbody>();
    }

    private IEnumerator ExplosionRoutine()
    {
        yield return null;
        new Explosion(transform.position, _explosionRadius, _explosionForce);
    }
}