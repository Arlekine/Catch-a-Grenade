using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Destractable
{
    public Action Destroyed;
    public Action PathEnded;

    [SerializeField] private int _health;
    [SerializeField] private CarMover _carMover;
    [SerializeField] private GameObject _unbroken;
    [SerializeField] private GameObject _broken;
    [SerializeField] private List<Rigidbody> _parts;
    [SerializeField] private List<Enemy> _passengers;
    [SerializeField] private ParticleSystem _explosion;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _health;
        _carMover.PathEnded += OnPathEnded;
    }

    private void OnPathEnded()
    {
        PathEnded?.Invoke();
    }

    public void Activate()
    {
        _carMover.Activate();
    }

    public override List<Rigidbody> Destract()
    {
        _currentHealth--;
        if (_currentHealth <= 0)
        {
            GetComponent<Collider>().enabled = false;
            _unbroken.SetActive(false);
            _broken.SetActive(true);

            var explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            explosion.transform.parent = transform.parent;

            _carMover.enabled = false;

            StartCoroutine(ExplosionRoutine());
            Destroyed?.Invoke();

            return _parts;
        }

        return new List<Rigidbody>();
    }
    
    private IEnumerator ExplosionRoutine()
    {
        yield return new WaitForFixedUpdate();

        var destractables = new List<Destractable>();

        foreach (var passenger in _passengers)
        {
            destractables.Add(passenger);
        }

        _audioSource.Play();
        new Explosion(transform.position, _explosionRadius, _explosionForce, destractables);
    }
}