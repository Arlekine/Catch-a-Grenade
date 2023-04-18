using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSBlock : MonoBehaviour
{
    [SerializeField] private int _targetFPS;
    private void Start()
    {
        Application.targetFrameRate = _targetFPS;
    }
}
