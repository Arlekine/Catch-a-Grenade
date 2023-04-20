using DG.Tweening;
using TMPro;
using UnityEngine;

public class FontSizeSelectionAnimation : UISelectionAnimationWithAnimaionData
{
    [Space]
    [SerializeField] private TextMeshProUGUI _target;
    [SerializeField] private float _selectionSize;
    [SerializeField] private float _deselectionSize;
    
    private Sequence _currentSequence;

    protected override void PlaySelectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        var fontSizeTween = DOTween.To(() => _target.fontSize, size => _target.fontSize = size, _selectionSize, _time).SetEase(_ease);
        _currentSequence.Append(fontSizeTween);
    }

    protected override void PlayDeselectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        var fontSizeTween = DOTween.To(() => _target.fontSize, size => _target.fontSize = size, _deselectionSize, _time).SetEase(_ease);
        _currentSequence.Append(fontSizeTween);
    }

    protected override void SetSelectedInstantly()
    {
        _target.fontSize = _selectionSize;
    }

    protected override void SetDeselectedInstantly()
    {
        _target.fontSize = _deselectionSize;
    }
}