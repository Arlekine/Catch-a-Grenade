using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _characterRoot;
    [SerializeField] private float _rootAngleMin;
    [SerializeField] private float _rootAngleMax;
    [Range(0, 1)][SerializeField] private float _rotate;

    public Animator Animator => _animator;

    public void SetRotate(float rotate)
    {
        _rotate = Mathf.Clamp01(rotate);
    }

    private void LateUpdate()
    {
        _animator.SetFloat("Blend", _rotate);
        transform.localEulerAngles = Vector3.up *Mathf.Lerp(_rootAngleMin, _rootAngleMax, _rotate);
    }
}
