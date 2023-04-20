using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private GameObject _startMenu;

    [Space]
    [SerializeField] private Joystick _joystick;

    [Space]
    [SerializeField] private Counter _grenades;
    [SerializeField] private Counter _enemies;
    [SerializeField] private Counter _cars;

    [Space]
    [SerializeField] private SwitchButton _hapticButton;
    [SerializeField] private SwitchButton _soundButton;

    [Space] 
    [SerializeField] private LevelEndPanel _loosePanel;
    [SerializeField] private LevelEndPanel _winPanel;
    
    public GameObject StartMenu => _startMenu;
    public Joystick Joystick => _joystick;
    public Counter Grenades => _grenades;
    public Counter Enemies => _enemies;
    public Counter Cars => _cars;

    public SwitchButton HapticButton => _hapticButton;
    public SwitchButton SoundButton => _soundButton;

    public LevelEndPanel LoosePanel => _loosePanel;
    public LevelEndPanel WinPanel => _winPanel;
}