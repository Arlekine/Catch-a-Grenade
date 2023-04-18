using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destractable : MonoBehaviour
{
    public abstract List<Rigidbody> Destract();
}