using UnityEngine;

public class Context : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private CameraCenterRotation _camera;

    public GameData GameData;

    public UI UI => _ui;
    public CameraCenterRotation Camera => _camera;

    public bool IsSoundOn => GameData.SoundOn;
    public bool IsHapticOn => GameData.HapticOn;
}