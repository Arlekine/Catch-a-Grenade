using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class CharacterControl : MonoBehaviour
{
    [SerializeField] private CharacterRotator _characterRotator;
    [SerializeField] private SpineRotation _spineRotation;
    [SerializeField] private GrenadeThrower _grenadeThrower;
    [SerializeField] private GrenadeTrajectoryDrawer _trajectoryDrawer;
    
    private CameraCenterRotation _cameraCenterRotation;
    private Joystick _joystick;
    private bool _isControlling;

    public GrenadeThrower GrenadeThrower => _grenadeThrower;

    public void Init(Joystick joystick, CameraCenterRotation cameraCenterRotation)
    {
        _joystick = joystick;
        _cameraCenterRotation = cameraCenterRotation;

        _grenadeThrower.SetControl(new Vector2(0.5f, 0.5f));
        _joystick.Pressed += () =>
        {
            _isControlling = true;
            _characterRotator.Animator.SetBool("IsAiming", _isControlling);
            _trajectoryDrawer.CreateLine();
            _grenadeThrower.ReturnGrenade();
        };

        _joystick.Released += () =>
        {
            _isControlling = false;
            _characterRotator.Animator.SetTrigger("Throw");
            _characterRotator.Animator.SetBool("IsAiming", _isControlling);
            _grenadeThrower.Throw(_trajectoryDrawer.DirectionAfterHits);
            _trajectoryDrawer.HideLine();
        };
    }

    private void OnDisable()
    {
        _joystick.Pressed = null;
        _joystick.Released = null;
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
            _cameraCenterRotation.Rotate(horizontal);
        }
        else
        {
            _spineRotation.SetAngle(0.5f);
            _characterRotator.SetRotate(0f);
            _cameraCenterRotation.Rotate(0.5f);
        }
    }
}