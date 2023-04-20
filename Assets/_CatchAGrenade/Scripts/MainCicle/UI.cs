using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _gamePlayMenu;

    [Space]
    [SerializeField] private Joystick _joystick;

    [Space]
    [SerializeField] private GrenadesAmountView _grenades;
    [SerializeField] private Counter _enemies;
    [SerializeField] private Counter _cars;

    [Space]
    [SerializeField] private SwitchButton _hapticButton;
    [SerializeField] private SwitchButton _soundButton;

    [Space] 
    [SerializeField] private LevelEndPanel _loosePanel;
    [SerializeField] private LevelEndPanel _winPanel;

    [Space] 
    [SerializeField] private GameObject _moveTutorial;
    [SerializeField] private GameObject _untapTutorial;
    
    public GameObject StartMenu => _startMenu;
    public GameObject GamePlayMenu => _gamePlayMenu;

    public Joystick Joystick => _joystick;
    public GrenadesAmountView Grenades => _grenades;
    public Counter Enemies => _enemies;
    public Counter Cars => _cars;

    public SwitchButton HapticButton => _hapticButton;
    public SwitchButton SoundButton => _soundButton;

    public LevelEndPanel LoosePanel => _loosePanel;
    public LevelEndPanel WinPanel => _winPanel;

    public GameObject MoveTutorial => _moveTutorial;
    public GameObject UntapTutorial => _untapTutorial;
}