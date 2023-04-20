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

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;

    public override List<Rigidbody> Destract()
    {
        GetComponent<Collider>().enabled = false;
        _model.enabled = false;

        var explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.parent = transform.parent;
        StartCoroutine(ExplosionRoutine());

        return new List<Rigidbody>();
    }

    private IEnumerator ExplosionRoutine()
    {
        yield return null;
        _audioSource.Play();
        new Explosion(transform.position, _explosionRadius, _explosionForce);
    }
}