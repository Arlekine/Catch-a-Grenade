using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singletone<SoundManager>
{
    [SerializeField] private AudioSource _uiClick;
    [SerializeField] private AudioSource _win;
    [SerializeField] private AudioSource _loose;

    public void PlayUIClick()
    {
        _uiClick.Play();
    }

    public void PlayWin()
    {
        _win.Play();
    }
    public void PlayLoose()
    {
        _loose.Play();
    }
}
