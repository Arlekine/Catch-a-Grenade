using UnityEngine;

public class ForceImpacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _force;

    public void ImpactForce()
    {
        _rigidbody.AddForce(_force);
    }
}