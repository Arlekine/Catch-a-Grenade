using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonClick : MonoBehaviour
{
    private Button _button;

    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button;
        }
    }

    private void OnEnable()
    {
        if (Button != null)
            Button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        if (Button != null)
            Button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        SoundManager.Instance.PlayUIClick();
    }
}
