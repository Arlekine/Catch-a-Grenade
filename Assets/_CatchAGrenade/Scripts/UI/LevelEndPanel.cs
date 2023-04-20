using System;
using UnityEngine;

public class LevelEndPanel : MonoBehaviour
{
    public Action NextClicked;

    [SerializeField] private UiShowingAnimation _animation;

    public void Open()
    {
        _animation.Show();
    }

    public void Close()
    {
        _animation.Hide();
    }

    public void Next()
    {
        Close();
        NextClicked?.Invoke();
    }
}