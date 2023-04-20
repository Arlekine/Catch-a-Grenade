using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Action Lost;
    public Action Win;

    [SerializeField] private int _grenadesForLevel;
    [SerializeField] private CharacterControl _characterControl;
    [SerializeField] private Enemy[] _enemiesToDestroy;
    [SerializeField] private Car[] _carsToDestroy;
    [SerializeField] private bool _loseOnCarPathEnd;

    private int _currentGrenades;
    private int _currentTargetEnemies;
    private int _currentTargetCars;

    private Context _context;

    public void Init(Context context)
    {
        _context = context;
        _currentGrenades = _grenadesForLevel;
        _currentTargetEnemies = _enemiesToDestroy.Length;
        _currentTargetCars = _carsToDestroy.Length;

        _context.UI.Joystick.enabled = true;
        _context.UI.Grenades.SetMaxValue(_grenadesForLevel);
        _context.UI.Enemies.SetMaxValue(_currentTargetEnemies);
        _context.UI.Cars.SetMaxValue(_currentTargetCars);

        _context.UI.Enemies.gameObject.SetActive(_currentTargetEnemies > 0);
        _context.UI.Cars.gameObject.SetActive(_currentTargetCars > 0);

        _characterControl.Init(context.UI.Joystick, context.Camera);
        _characterControl.GrenadeThrower.SetData(_context.GameData);

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
    }

    private void OnGrenadeThrowed()
    {
        _currentGrenades--;

        _context.UI.Joystick.enabled = false;
        _context.UI.Grenades.SetCurrentValue(_currentGrenades);
    }

    private void OnGrenadeExploded()
    {
        if (_currentGrenades <= 0)
        {
            InvokeLoose();
        }
        else
        {
            _context.UI.Joystick.enabled = true;
        }
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
        _context.UI.Joystick.enabled = false;
        Lost?.Invoke();
    }

    private void InvokeWin()
    {
        _context.UI.Joystick.enabled = false;
        Win?.Invoke();
    }
}