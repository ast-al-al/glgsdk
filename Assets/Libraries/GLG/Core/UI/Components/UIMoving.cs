using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMoving : MonoBehaviour
{
    public RectTransform rectTransform;
    public Vector2 startPosition;
    public Vector2 moveTo;
    public float duration = 1f;
    public float delay = 1f;
    public Ease ease = Ease.InOutCirc;
    public bool onEnable = true;
    public UnityEvent onComplete;

#if UNITY_EDITOR
    private void OnValidate()
    {
        rectTransform = GetComponent<RectTransform>();
    }
#endif
    private void OnEnable()
    {
        if (onEnable)
        {
            Play();
        }
    }
    public void Play()
    {
        rectTransform.DOKill();
        rectTransform.anchoredPosition = startPosition;
        rectTransform.DOAnchorPos(moveTo, duration)
            .SetDelay(delay)
            .SetEase(ease)
            .OnComplete(() => { onComplete.Invoke(); });
    }
    public void PlayBackwards(bool withDelay)
    {
        rectTransform.DOKill();
        rectTransform.anchoredPosition = moveTo;
        rectTransform.DOAnchorPos(startPosition, duration)
            .SetDelay(withDelay ? delay : 0f)
            .SetEase(ease)
            .OnComplete(() => { onComplete.Invoke(); });
    }
    public void PlayBackwardsSilent(bool withDelay)
    {
        rectTransform.DOKill();
        rectTransform.anchoredPosition = moveTo;
        rectTransform.DOAnchorPos(startPosition, duration)
            .SetDelay(withDelay ? delay : 0f)
            .SetEase(ease);
    }
}
