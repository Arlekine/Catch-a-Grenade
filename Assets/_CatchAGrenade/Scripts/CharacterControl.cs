using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private CharacterRotator _characterRotator;
    [SerializeField] private SpineRotation _spineRotation;
    [SerializeField] private GrenadeThrower _grenadeThrower;
    [SerializeField] private GrenadeTrajectoryDrawer _trajectoryDrawer;
    [SerializeField] private Joystick _joystick;

    private bool _isControlling;

    private void Start()
    {
        _joystick.Pressed += () =>
        {
            _isControlling = true;
            _characterRotator.Animator.SetBool("IsAiming", _isControlling);
            _trajectoryDrawer.CreateLine();
            _grenadeThrower.ReturnGrenade();
        };

        _joystick.Released += () =>
        {
            if (_characterRotator.Animator == null)
                return;

            _isControlling = false;
            _characterRotator.Animator.SetTrigger("Throw");
            _characterRotator.Animator.SetBool("IsAiming", _isControlling);
            _grenadeThrower.Throw(_trajectoryDrawer.GetTrajectory());
            _trajectoryDrawer.HideLine();
        };
    }

    private void Update()
    {
        if (_isControlling)
        {
            var horizontal = _joystick.Horizontal * 0.5f + 0.5f;
            var vertical = 1 - (_joystick.Vertical * 0.5f + 0.5f);

            _spineRotation.SetAngle(horizontal);
            _characterRotator.SetRotate(vertical);

            _grenadeThrower.SetControl(new Vector2(horizontal, vertical));
        }
        else
        {
            _spineRotation.SetAngle(0.5f);
            _characterRotator.SetRotate(0f);
        }
    }
}