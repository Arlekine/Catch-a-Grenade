using System.Collections.Generic;
using UnityEngine;

public class Wall : Destractable
{
    [SerializeField] protected GameObject _fullPart;
    [SerializeField] protected List<Rigidbody> _parts = new List<Rigidbody>();

    public override List<Rigidbody> Destract()
    {
        GetComponent<Collider>().enabled = false;
        _fullPart.SetActive(false);

        foreach (var part in _parts)
        {
            part.gameObject.SetActive(true);
        }
        return _parts;
    }
}