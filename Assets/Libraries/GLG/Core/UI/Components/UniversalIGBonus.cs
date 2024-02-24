using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalIGBonus : MonoBehaviour
{
    public string bonusName;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _adIcon;
    [SerializeField] private RectTransform _rectTransform;
    public System.Action<bool> onPressed;
   
    public bool IsActivated { get; private set; }
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = _rectTransform.anchoredPosition;
    }
    public void Show(bool withAd)
    {
        _rectTransform.DOKill();
        _rectTransform.anchoredPosition = _startPosition;
        gameObject.SetActive(true);
        _adIcon.SetActive(withAd);
        Vector2 targetPos = _startPosition;
        targetPos.x = 0f;
        _rectTransform.DOAnchorPos(targetPos, 2f).SetEase(Ease.OutElastic, 1.1f);
        IsActivated = true;
    }
    public void Hide()
    {
        _rectTransform.DOKill();
        gameObject.SetActive(false);
        IsActivated = false;
    }

    public void OnPressed()
    {
        if (_adIcon.activeSelf)
        {
            onPressed?.Invoke(true);
        }
        else
        {
            onPressed?.Invoke(false);
        }
    }
    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}
