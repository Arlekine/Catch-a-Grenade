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
            var rawInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
            //rawInput = Quaternion.Euler(0f, 0f, 45f) * rawInput;

            var horizontal = 1 - (rawInput.x * 0.5f + 0.5f);
            var vertical = (rawInput.y * 0.5f + 0.5f);

            var input = new Vector2(horizontal, vertical);
            print(rawInput.x + " " + rawInput.y);

            _spineRotation.SetAngle(input.x);
            _characterRotator.SetRotate(input.y);

            _grenadeThrower.SetControl(new Vector2(input.x, input.y));
            _cameraCenterRotation.Rotate(input.x);
        }
        else
        {
            _spineRotation.SetAngle(0.5f);
            _characterRotator.SetRotate(0f);
            _cameraCenterRotation.Rotate(0.5f);
        }
    }
}