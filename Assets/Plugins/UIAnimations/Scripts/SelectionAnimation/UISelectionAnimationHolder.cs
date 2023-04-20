using System;
using UnityEngine;

public class UISelectionAnimationHolder : UISelectionAnimation
{
    [SerializeField] private UISelectionAnimation[] _animations;

    [EditorButton]
    protected override void PlaySelectionAnimation()
    {
        SetToAll((selectionAnimation => selectionAnimation.Select()));
    }

    [EditorButton]
    protected override void PlayDeselectionAnimation()
    {
        SetToAll((selectionAnimation => selectionAnimation.Deselect()));
    }

    protected override void SetSelectedInstantly()
    {
        SetToAll((selectionAnimation => selectionAnimation.SelectInstantly()));
    }

    protected override void SetDeselectedInstantly()
    {
        SetToAll((selectionAnimation => selectionAnimation.DeselectInstantly()));
    }

    private void SetToAll(Action<UISelectionAnimation> action)
    {
        foreach (var anim in _animations)
        {
            action(anim);
        }
    }
}