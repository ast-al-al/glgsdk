using DG.Tweening;
using GLG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyOverlay : UIController
{
    [SerializeField] private Text _text;
    [SerializeField] private RectTransform _window;

    protected override void OnStartShow()
    {
        _window.localPosition = new Vector3(0f, -100f, 0f);
        _window.DOLocalMove(Vector3.zero, 1.5f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Hide();
        });
    }
    public NotifyOverlay SetText(string text)
    {
        _text.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_window);
        return this;
    }
}
