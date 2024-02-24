using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;


public class UniversalButton : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _unlockedUI;
    [SerializeField] private GameObject _lockedUI;
    [Header("Texts")]
    [SerializeField] private Text _unlockedText;
    [SerializeField] private Text _lockedText;
    [Header("Icons")]
    [SerializeField] private Image _unlockedIcon;
    [SerializeField] private Image _lockedIcon;
    [SerializeField] private GameObject _adIconBlock;
    [Header("Backgrounds")]
    [SerializeField] private Image _unlockedBackground;
    [SerializeField] private Image _lockedBackground;


    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _lockedColor;

    public RectTransform RectTransform => _rectTransform;
    public bool IsLocked { get; private set; }

    public UnityEvent onCkicked;
    public event System.Action onClickedEvent;


    public UniversalButton SetBackgroundColor(Color color)
    {
        if (IsLocked)
        {
            if (_lockedBackground) _lockedBackground.color = color;
        }
        else
        {
            if (_unlockedBackground) _unlockedBackground.color = color;
        }
        return this;
    }

    #region HEADER
    public UniversalButton SetHeader(string text, bool bothLockedAndUnlocked = false)
    {
        if (IsLocked || bothLockedAndUnlocked)
        {
            if (_lockedText)
            {
                _lockedText.text = text;
            }
        }
        if (!IsLocked || bothLockedAndUnlocked)
        {
            if (_unlockedText)
            {
                _unlockedText.text = text;
            }
        }
        return this;
    }
    public UniversalButton SetHeaderVisible(bool state)
    {
        if (IsLocked)
        {
            if (_lockedText) _lockedText.gameObject.SetActive(state);
        }
        else
        {
            if (_unlockedText) _unlockedText.gameObject.SetActive(state);
        }
        return this;
    }
    public UniversalButton SetHeaderColor(Color color)
    {
        if (IsLocked)
        {
            if (_lockedText) _lockedText.color = color;
        }
        else
        {
            if (_unlockedText) _unlockedText.color = color;
        }
        return this;
    }
    #endregion

    #region ICON
    public UniversalButton SetIcon(Sprite sprite)
    {
        if (IsLocked)
        {
            if (_lockedIcon) _lockedIcon.sprite = sprite;
        }
        else
        {
            if (_unlockedIcon) 
            {
                _unlockedIcon.sprite = sprite;
                _unlockedIcon.gameObject.SetActive(true);
                _adIconBlock.gameObject.SetActive(false);
            }
        }
        return this;
    }
    public UniversalButton SetIcon(SpriteAtlas spriteAtlas, string spriteName)
    {
        if (IsLocked)
        {
            if (_lockedIcon) _lockedIcon.sprite = spriteAtlas.GetSprite(spriteName);
        }
        else
        {
            if (_unlockedIcon)
            {
                _unlockedIcon.sprite = spriteAtlas.GetSprite(spriteName);
                _unlockedIcon.gameObject.SetActive(true);
                _adIconBlock.gameObject.SetActive(false);
            }
        }
        return this;
    }
    public UniversalButton SetIconColor(Color color)
    {
        if (IsLocked)
        {
            if (_lockedIcon) _lockedIcon.color = color;
        }
        else
        {
            if (_unlockedIcon) _unlockedIcon.color = color;
        }
        return this;
    }
    public UniversalButton SetIconVisible(bool state)
    {
        if (IsLocked)
        {
            if (_lockedIcon) _lockedIcon.gameObject.SetActive(state);
        }
        else
        {
            if (_unlockedIcon) _unlockedIcon.gameObject.SetActive(state);
        }
        return this;
    }
    #endregion

    #region LOCK
    public UniversalButton SetLocked(bool locked, bool withText = true)
    {
        
            IsLocked = locked;
            if (withText)
            {
                if (_lockedUI) _lockedUI.gameObject.SetActive(locked);
                if (_unlockedUI) _unlockedUI.gameObject.SetActive(!locked);
            }
            else
            {
                if (_unlockedBackground) _unlockedBackground.color = locked ? _lockedColor : _normalColor;
            }
        
        return this;
    }
    #endregion



    public void DoClick()
    {
        if (!IsLocked)
        {
            onClickedEvent?.Invoke();
            onCkicked.Invoke();
        }
    }
}
