using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class PopupAnimation
{
    [Header("Animation Settings")]
    public float animationTime = 0.5f;
    public Ease animationType = Ease.OutBack;
    public Vector3 startScale = Vector3.zero;
    public Vector3 endScale = Vector3.one;

    public void PlayAnimation(Transform popup, Action onComplete = null)
    {
        popup.localScale = startScale;
        popup
            .DOScale(endScale, animationTime)
            .SetEase(animationType)
            .OnComplete(() => onComplete?.Invoke())
            .SetAutoKill(true);
    }

    public void ReverseAnimation(Transform popup, Action onComplete = null)
    {
        popup
            .DOScale(startScale, animationTime)
            .SetEase(animationType)
            .OnComplete(() => onComplete?.Invoke())
            .SetAutoKill(true);
    }
}
