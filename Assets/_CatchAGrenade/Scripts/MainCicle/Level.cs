using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Action Lost;
    public Action Win;

    [SerializeField] private int _grenadesForLevel;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private Zone _levelZone;
    [SerializeField] private Enemy[] _enemiesToDestroy;
    [SerializeField] private Car[] _carsToDestroy;
    [SerializeField] private bool _loseOnCarPathEnd;

    private int _currentGrenades;
    private int _currentTargetEnemies;
    private int _currentTargetCars;

    private Context _context;
    private bool _isLevelEnded;

    public void Init(Context context)
    {
        _context = context;
        _currentGrenades = _grenadesForLevel;
        _currentTargetEnemies = _enemiesToDestroy.Length;
        _currentTargetCars = _carsToDestroy.Length;

        _isLevelEnded = false;
        _characterControl.IsControlling = true;
        _context.UI.Grenades.SetMaxValue(_grenadesForLevel);
        _context.UI.Enemies.SetMaxValue(_currentTargetEnemies);
        _context.UI.Cars.SetMaxValue(_currentTargetCars);

        _context.UI.Enemies.gameObject.SetActive(_currentTargetEnemies > 0);
        _context.UI.Cars.gameObject.SetActive(_currentTargetCars > 0);

        _characterControl.Init(context.Camera);
        _characterControl.GrenadeThrower.SetData(_context.GameData);

        _characterControl.GrenadeThrower.Grenade.SetZone(_levelZone);

        foreach (var enemy in _enemiesToDestroy)
        {
            enemy.Dead += OnEnemyDead;
        }

        foreach (var car in _carsToDestroy)
        {
            car.Destroyed += OnCarDestroyed;
            car.PathEnded += OnCarPathEnded;
        }

        _characterControl.GrenadeThrower.GrenadeThrowed += OnGrenadeThrowed;
        _characterControl.GrenadeThrower.GrenadeExploded += OnGrenadeExploded;

        if (_context.GameData.IsTutorial)
        {
            _context.UI.MoveTutorial.SetActive(true);
            LeanTouch.OnFingerDown += ShowUptapTutorial;
        }
    }

    public void StartLevel()
    {
        foreach (var car in _carsToDestroy)
        {
            car.Activate();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_levelZone.Center, _levelZone.Radius);
    }

    private void ShowUptapTutorial(LeanFinger finger)
    {
        LeanTouch.OnFingerDown -= ShowUptapTutorial;
        _context.UI.MoveTutorial.SetActive(false);
        _context.UI.UntapTutorial.SetActive(true);
    }

    private void OnGrenadeThrowed()
    {
        _currentGrenades--;

        if (_context.GameData.IsTutorial)
        {
            _context.UI.UntapTutorial.SetActive(false);
            _context.GameData.IsTutorial = false;
            _context.SaveData();
        }

        _characterControl.IsControlling = false;
        _context.UI.Grenades.SetCurrentValue(_currentGrenades);
    }

    private void OnGrenadeExploded()
    {
        if (_currentGrenades <= 0)
        {
            StartCoroutine(CheckLooseRoutine());
        }
        else
        {
            _characterControl.IsControlling = true;
        }
    }

    private IEnumerator CheckLooseRoutine()
    {
        yield return null;
        yield return null;

        if (_currentTargetEnemies > 0 || _currentTargetCars > 0)
            InvokeLoose();
    }

    private void OnCarPathEnded()
    {
        if (_loseOnCarPathEnd)
            InvokeLoose();
    }

    private void OnCarDestroyed()
    {
        _currentTargetCars--;
        _context.UI.Cars.SetCurrentValue(_currentTargetCars);

        if (_currentTargetEnemies <= 0 && _currentTargetCars <= 0)
            InvokeWin();
    }

    private void OnEnemyDead()
    {
        _currentTargetEnemies--;
        _context.UI.Enemies.SetCurrentValue(_currentTargetEnemies);

        if (_currentTargetEnemies <= 0 && _currentTargetCars <= 0)
            InvokeWin();
    }
    private void InvokeLoose()
    {
        if (_isLevelEnded)
            return;

        _isLevelEnded = true;
        _characterControl.IsControlling = false;
        Lost?.Invoke();
    }

    private void InvokeWin()
    {
        if (_isLevelEnded)
            return;

        _isLevelEnded = true;
        _characterControl.IsControlling = false;
        Win?.Invoke();
    }
}