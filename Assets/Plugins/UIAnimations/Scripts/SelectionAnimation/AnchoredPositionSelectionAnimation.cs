using DG.Tweening;
using UnityEngine;

public class AnchoredPositionSelectionAnimation : UISelectionAnimationWithAnimaionData
{
    [Space]
    [SerializeField] private RectTransform _target;
    [SerializeField] private Vector2 _selectionPosition;
    [SerializeField] private Vector2 _deselectionPosition;
    
    private Sequence _currentSequence;

    protected override void PlaySelectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _currentSequence.Append(_target.DOAnchorPos(_selectionPosition, _time).SetEase(_ease));
    }

    protected override void PlayDeselectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _currentSequence.Append(_target.DOAnchorPos(_deselectionPosition, _time).SetEase(_ease));
    }

    protected override void SetSelectedInstantly()
    {
        _target.anchoredPosition = _selectionPosition;
    }

    protected override void SetDeselectedInstantly()
    {
        _target.localScale = Vector3.one *_deselectionPosition;
    }
}