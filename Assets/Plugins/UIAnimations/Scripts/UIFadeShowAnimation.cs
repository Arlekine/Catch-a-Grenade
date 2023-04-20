using System;
using DG.Tweening;
using UnityEngine;

public class UIFadeShowAnimation : UiShowingAnimation
{
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected float _showTime;
    [SerializeField] protected bool _switchInteractable = true;

    public override bool IsShowed => _canvasGroup.interactable;

    private void Awake()
    {
        HideInstantly();
    }

    public override void Show()
    {
        SetCanvasGroupInteractable(true);
        _canvasGroup.DOFade(1f, _showTime);
    }

    public override void Hide()
    {
        SetCanvasGroupInteractable(false);
        _canvasGroup.DOFade(0f, _showTime);
    }

    public override void ShowInstantly()
    {
        SetCanvasGroupInteractable(true);
        _canvasGroup.alpha = 1f;
    }

    public override void HideInstantly()
    {
        SetCanvasGroupInteractable(false);
        _canvasGroup.alpha = 0f;
    }

    private void SetCanvasGroupInteractable(bool isInteractable)
    {
        if (_switchInteractable)
        {
            _canvasGroup.interactable = isInteractable;
            _canvasGroup.blocksRaycasts = isInteractable;
        }
    }
}