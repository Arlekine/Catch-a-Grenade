using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectionAnimation : UISelectionAnimationWithAnimaionData
{
    [Space]
    [SerializeField] private Color _selectionColor;
    [SerializeField] private Color _deselectionColor;
    
    [Space]
    [SerializeField] private Graphic[] _graphics;

    private Sequence _currentSequence;
    
    public Color SelectionColor
    {
        get => _selectionColor;
        set
        {
            _selectionColor = value;
            if (IsSelected)
                Select();
        }
    }
    
    public Color DeselectionColor 
    {
        get => _deselectionColor;
        set
        {
            _deselectionColor = value;
            if (IsSelected == false)
                Deselect();
        }
    }

    protected override void PlaySelectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        
        foreach (var graphic in _graphics)
        {
            _currentSequence.Join(graphic.DOColor(SelectionColor, _time));
        }
    }

    protected override void PlayDeselectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        foreach (var graphic in _graphics)
        {
            _currentSequence.Join(graphic.DOColor(DeselectionColor, _time));
        }
    }

    protected override void SetSelectedInstantly()
    {
        foreach (var graphic in _graphics)
        {
            graphic.color = SelectionColor;
        }
    }

    protected override void SetDeselectedInstantly()
    {
        foreach (var graphic in _graphics)
        {
            graphic.color = DeselectionColor;
        }
    }
}