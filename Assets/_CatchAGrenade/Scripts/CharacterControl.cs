using System.Collections;
using Lean.Touch;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class CharacterControl : MonoBehaviour
{
    public bool IsControlling;

    [SerializeField] private CharacterRotator _characterRotator;
    [SerializeField] private SpineRotation _spineRotation;
    [SerializeField] private GrenadeThrower _grenadeThrower;
    [SerializeField] private GrenadeTrajectoryDrawer _trajectoryDrawer;
    [SerializeField] private float _screenMovingParameter = 0.1f;

    private CameraCenterRotation _cameraCenterRotation;
    private LeanFinger _currentFinger;
    private bool _canThrow;

    public GrenadeThrower GrenadeThrower => _grenadeThrower;

    private Vector2 _currentInput;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Init(CameraCenterRotation cameraCenterRotation)
    {
        _cameraCenterRotation = cameraCenterRotation;

        _grenadeThrower.SetControl(new Vector2(0.5f, 0.5f));
        _currentInput = new Vector2(0.5f, 0.5f);

        LeanTouch.OnFingerDown += FingerDown;
        LeanTouch.OnFingerUp += FingerUp;
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= FingerDown;
        LeanTouch.OnFingerUp -= FingerUp;

    }

    private void FingerDown(LeanFinger finger)
    {
        if (_currentFinger != null || IsControlling == false || gameManager._startGame == false)
            return;

        _canThrow = false;
        _currentFinger = finger;
        _characterRotator.Animator.SetBool("IsAiming", true);
        _trajectoryDrawer.CreateLine();
        _grenadeThrower.ReturnGrenade();

        StopAllCoroutines();
        StartCoroutine(ActivateControl());
    }

    private IEnumerator ActivateControl()
    {
        yield return new WaitForSeconds(0.3f);
        _canThrow = true;
    }

    private void FingerUp(LeanFinger finger)
    {
        if (finger != _currentFinger)
            return;

        _currentFinger = null;

        if (_canThrow)
        {
            _characterRotator.Animator.SetTrigger("Throw");
            _grenadeThrower.Throw(_trajectoryDrawer.DirectionAfterHits);
        }

        _characterRotator.Animator.SetBool("IsAiming", false);
        _trajectoryDrawer.HideLine();
    }

    private void Update()
    {
        if (IsControlling && _currentFinger != null)
        {
            var rawDelta = _currentFinger.ScaledDelta;
            rawDelta.x = -rawDelta.x;

            _currentInput += rawDelta * _screenMovingParameter;
            _currentInput.x = Mathf.Clamp01(_currentInput.x);
            _currentInput.y = Mathf.Clamp01(_currentInput.y);

            _spineRotation.SetAngle(_currentInput.x);
            _characterRotator.SetRotate(_currentInput.y);

            _grenadeThrower.SetControl(new Vector2(_currentInput.x, _currentInput.y));
            _cameraCenterRotation.Rotate(_currentInput.x);
        }
        else
        {
            _spineRotation.SetAngle(0.5f);
            _characterRotator.SetRotate(0f);
            _cameraCenterRotation.Rotate(0.5f);
        }
    }
}