using System.Collections.Generic;
using UnityEngine;

public class ConcreteWall : Wall
{
    [SerializeField] private MeshRenderer _mainPart;
    [SerializeField] private Material _damagedMaterial;
    [SerializeField] private ParticleSystem _damageParticles;

    private bool _isDamaged;

    private void Start()
    {
        foreach (var part in _parts)
        {
            part.gameObject.SetActive(false);
        }
    }

    public override List<Rigidbody> Destract()
    {
        if (_isDamaged == false)
        {
            _mainPart.material = _damagedMaterial;
            _damageParticles.Play();
            _isDamaged = true;
            return new List<Rigidbody>();
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            _fullPart.gameObject.SetActive(false);

            foreach (var part in _parts)
            {
                part.gameObject.SetActive(true);
            }
            
            return _parts;
        }
    }
}