using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineRotation : MonoBehaviour
{
    [SerializeField] private float _angle;
    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;

    public void SetAngle(float angleNormilized)
    {
        _angle = Mathf.Lerp(_angleMin, _angleMax, angleNormilized);
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = Vector3.right * _angle;
    }
}