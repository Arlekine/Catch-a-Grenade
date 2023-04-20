using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _currentValue;
    private int _maxValue;

    public void SetMaxValue(int value)
    {
        _maxValue = _currentValue = value;
        UpdateText();
    }

    public void SetCurrentValue(int value)
    {
        if (value > _maxValue)
            throw new ArgumentException($"{nameof(value)} should be less than {nameof(_maxValue)}");

        _currentValue = value;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"{_currentValue}/{_maxValue}";
    }
}