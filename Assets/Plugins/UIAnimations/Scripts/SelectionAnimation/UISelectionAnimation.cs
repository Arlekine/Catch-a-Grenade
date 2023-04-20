using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISelectionAnimation : MonoBehaviour
{
    public bool IsSelected { get; private set; }

    public void Select()
    {
        IsSelected = true;
        PlaySelectionAnimation();
    }

    public void Deselect()
    {
        IsSelected = false;
        PlayDeselectionAnimation();
    }

    public void SelectInstantly()
    {
        IsSelected = true;
        SetSelectedInstantly();
        
    }

    public void DeselectInstantly()
    {
        IsSelected = false;
        SetDeselectedInstantly();
    }

    protected abstract void PlaySelectionAnimation();
    protected abstract void PlayDeselectionAnimation();
    
    protected abstract void SetSelectedInstantly();
    protected abstract void SetDeselectedInstantly();
}