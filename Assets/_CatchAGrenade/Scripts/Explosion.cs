using System.Collections.Generic;
using UnityEngine;

public class Explosion
{
    private Vector3 _point;
    private float _radius;
    private float _force;
    private List<Destractable> _shouldDestroy;

    public Explosion(Vector3 point, float radius, float force, List<Destractable> shouldDestroy = null)
    {
        _point = point;
        _radius = radius;
        _force = force;
        _shouldDestroy = shouldDestroy;

        Explode();
    }

    private void Explode()
    {
        var colliders = Physics.OverlapSphere(_point, _radius);
        var rigidbodies = new HashSet<Rigidbody>();

        if (_shouldDestroy != null)
        {
            foreach (var destractable in _shouldDestroy)
            {
                var bodies = destractable.Destract();
                foreach (var body  in bodies)
                {
                    rigidbodies.Add(body);
                }
            }
        }

        foreach (var collider in colliders)
        {
            var destractable = collider.GetComponent<Destractable>();

            if (destractable != null)
            {
                var destractableRigidbodies = destractable.Destract();
                foreach (var destractableRigidbody in destractableRigidbodies)
                {
                    rigidbodies.Add(destractableRigidbody);
                }
            }
            else if (collider.attachedRigidbody != null)
            {
                rigidbodies.Add(collider.attachedRigidbody);
            }
        }

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(_force, _point, _radius);
        }
    }
}