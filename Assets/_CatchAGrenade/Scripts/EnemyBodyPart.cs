using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : Destractable
{
    public override List<Rigidbody> Destract()
    {
        return GetComponentInParent<Enemy>().Destract();
    }
}