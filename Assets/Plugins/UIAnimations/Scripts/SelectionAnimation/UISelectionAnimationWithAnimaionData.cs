using DG.Tweening;
using UnityEngine;

public abstract class UISelectionAnimationWithAnimaionData : UISelectionAnimation
{
    [SerializeField] protected float _time = 0.3f;
    [SerializeField] protected Ease _ease = Ease.Linear;
}