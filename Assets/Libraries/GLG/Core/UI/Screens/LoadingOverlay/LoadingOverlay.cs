using DG.Tweening;
using GLG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingOverlay : UIController
{
    [SerializeField] private RectTransform _loadingImageRectTransform;
    Sequence s;
    protected override void OnStartShow()
    {
        s.Kill();
        s = DOTween.Sequence();
        s.SetDelay(0.1f);
        s.Append(_loadingImageRectTransform.DOLocalRotate(Vector3.forward * 30f, 0.001f)
            .SetRelative());
        s.AppendInterval(0.1f);
        s.SetLoops(-1, LoopType.Incremental);
    }
    protected override void OnEndHide()
    {
        s.Kill();
        _loadingImageRectTransform.DOKill();
    }
}
