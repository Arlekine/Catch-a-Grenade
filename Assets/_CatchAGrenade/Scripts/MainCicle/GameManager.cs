using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string GameDataPlayerPrefs = "GameData";

    [SerializeField] private Context _context;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Level[] _levels;

    private GameData _gameData;
    private Level _currentLevel;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(GameDataPlayerPrefs))
        {
            _gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(GameDataPlayerPrefs));
        }
        else
        {
            _gameData = new GameData();
            SaveData();
        }

        if (_levels.Length < _gameData.CurrentLevel)
        {
            _gameData.CurrentLevel = 0;
            SaveData();
        }

        AudioListener.volume = _gameData.SoundOn ? 1f : 0f;
        _context.GameData = _gameData;

        _context.UI.HapticButton.SetState(_gameData.HapticOn);
        _context.UI.SoundButton.SetState(_gameData.SoundOn);

        _context.UI.HapticButton.OnSwitch += HapticSwitch;
        _context.UI.SoundButton.OnSwitch += SoundSwitch;

        _context.UI.WinPanel.NextClicked += LoadLevel;
        _context.UI.LoosePanel.NextClicked += LoadLevel;
    }

    public void StartGame()
    {
        _context.UI.StartMenu.SetActive(false);
        LoadLevel();
    }

    public void HapticSwitch(bool isActive)
    {
        _gameData.HapticOn = isActive;
        SaveData();
    }

    public void SoundSwitch(bool isActive)
    {
        _gameData.SoundOn = isActive;
        AudioListener.volume = _gameData.SoundOn ? 1f : 0f;
        SaveData();
    }

    private void LoadLevel()
    {
        if (_currentLevel != null)
        {
            _currentLevel.Win = null;
            _currentLevel.Lost = null;
            Destroy(_currentLevel.gameObject);
        }
        
        _currentLevel = Instantiate(_levels[_gameData.CurrentLevel]);

        _currentLevel.Init(_context);
        _currentLevel.Lost += OnLevelLost;
        _currentLevel.Win += OnLevelWin;
    }

    private void OnLevelWin()
    {
        _gameData.CurrentLevel++;

        if (_gameData.CurrentLevel >= _levels.Length)
            _gameData.CurrentLevel = 0;

        SaveData();
        StartCoroutine(LevelLoadRoutine(true));
    }

    private void OnLevelLost()
    {

        StartCoroutine(LevelLoadRoutine(false));
    }

    private void SaveData()
    {
        PlayerPrefs.SetString(GameDataPlayerPrefs, JsonUtility.ToJson(_gameData));
    }

    private IEnumerator LevelLoadRoutine(bool win)
    {
        yield return new WaitForSeconds(2.5f);

        if (win)
        {
            _context.UI.WinPanel.Open();

            if (_gameData.HapticOn)
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            SoundManager.Instance.PlayWin();
        }
        else
        {
            if (_gameData.HapticOn)
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            SoundManager.Instance.PlayLoose();

            _context.UI.LoosePanel.Open();
        }
    }
}