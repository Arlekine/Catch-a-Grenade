using DG.Tweening;
using UnityEngine;

public class ScaleSelectionAnimation : UISelectionAnimationWithAnimaionData
{
    [Space]
    [SerializeField] private Transform _target;
    [SerializeField] private float _selectionSize;
    [SerializeField] private float _deselectionSize;
    
    private Sequence _currentSequence;

    protected override void PlaySelectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _currentSequence.Append(_target.DOScale(_selectionSize, _time).SetEase(_ease));
    }

    protected override void PlayDeselectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        _currentSequence.Append(_target.DOScale(_deselectionSize, _time).SetEase(_ease));
    }

    protected override void SetSelectedInstantly()
    {
        _target.localScale = Vector3.one *_selectionSize;
    }

    protected override void SetDeselectedInstantly()
    {
        _target.localScale = Vector3.one *_deselectionSize;
    }
}