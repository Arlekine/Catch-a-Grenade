using DG.Tweening;
using TMPro;
using UnityEngine;

public class SmoothBoldTextSelection : UISelectionAnimationWithAnimaionData
{
    [SerializeField] private CanvasGroup _thinText;
    [SerializeField] private CanvasGroup _boldText;

    private Sequence _currentSequence;
    protected override void PlaySelectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        
        _currentSequence.Join(_thinText.DOFade(0f, _time));
        _currentSequence.Join(_boldText.DOFade(1f, _time));
    }

    protected override void PlayDeselectionAnimation()
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        
        _currentSequence.Join(_thinText.DOFade(1f, _time));
        _currentSequence.Join(_boldText.DOFade(0f, _time));
    }

    protected override void SetSelectedInstantly()
    {
        SetTextAlpha(0f, 1f);
    }

    protected override void SetDeselectedInstantly()
    {
        SetTextAlpha(1f, 0f);
    }

    private void SetTextAlpha(float thinAlpha, float boldAlpha)
    {
        _thinText.alpha = thinAlpha;
        _boldText.alpha = boldAlpha;
    }
}