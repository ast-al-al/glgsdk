using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UniversalShopBlock : MonoBehaviour
{
    public int id;
    [SerializeField] private GameObject _unlockedUI;
    [SerializeField] private GameObject _lockedUI;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _lockedColor;
    [Header("Buttons")]
    public UniversalButton buyButton;
    public UniversalButton buyAddButton;
    [Header("Levels")]
    [SerializeField] private Text _currentLevel;
    [SerializeField] private Text _nextLevel;
    [SerializeField] private GameObject _arrow;
    [Header("Texts")]
    [SerializeField] private Text _unlockedText;
    [SerializeField] private Text _lockedText;
    [Header("Icons")]
    [SerializeField] private Image _unlockedIcon;
    [SerializeField] private Image _lockedIcon;
    [Header("Backgrounds")]
    [SerializeField] private Image _unlockedBackground;
    [SerializeField] private Image _lockedBackground;
    [Header("Selection")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _deselectedSprite;
    public bool IsLocked { get; private set; }

    public event System.Action<int> onBuyButtonClicked;
    public event System.Action<int> onAdButtonClicked;

    #region SELECTION
    public void Select()
    {
        _unlockedBackground.sprite = _selectedSprite;
        _lockedBackground.sprite = _selectedSprite;
    }
    public void Deselect()
    {
        _unlockedBackground.sprite = _deselectedSprite;
        _lockedBackground.sprite = _deselectedSprite;
    }
    #endregion

    #region LEVELS
    public UniversalShopBlock SetCurrentLevel(string currentLevelText)
    {
        if (_currentLevel) _currentLevel.text = currentLevelText;
        return this;
    }
    public UniversalShopBlock SetNextLevel(string nextLevelText)
    {
        if (_nextLevel) _nextLevel.text = nextLevelText;
        return this;
    }
    public UniversalShopBlock SetCurrenLevelVisible(bool visible)
    {
        if (_currentLevel) _currentLevel.gameObject.SetActive(visible);
        return this;
    }
    public UniversalShopBlock SetNextLevelVisible(bool visible)
    {
        if (_nextLevel) _nextLevel.gameObject.SetActive(visible);
        return this;
    }
    public UniversalShopBlock SetLevelArrowVisible(bool visible)
    {
        if (_arrow) _arrow.gameObject.SetActive(visible);
        return this;
    }
    #endregion

    #region HEADER
    public UniversalShopBlock SetHeader(string text)
    {
        if (IsLocked)
        {
            if (_lockedText)
            {
                _lockedText.text = text;
            }
        }
        else
        {
            if (_unlockedText)
            {
                _unlockedText.text = text;
            }
        }
        return this;
    }
    public UniversalShopBlock SetHeaderVisible(bool state)
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
    public UniversalShopBlock SetHeaderColor(Color color)
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
    public UniversalShopBlock SetIcon(Sprite sprite, bool bothLockedAndUnlocked = false)
    {
        if (IsLocked || bothLockedAndUnlocked)
        {
            if (_lockedIcon) _lockedIcon.sprite = sprite;
        }
        if (!IsLocked || bothLockedAndUnlocked)
        {
            if (_unlockedIcon) _unlockedIcon.sprite = sprite;
        }
        return this;
    }
    public UniversalShopBlock SetIcon(SpriteAtlas spriteAtlas, string spriteName, bool bothLockedAndUnlocked = false)
    {
        if (IsLocked || bothLockedAndUnlocked)
        {
            if (_lockedIcon) _lockedIcon.sprite = spriteAtlas.GetSprite(spriteName);
        }
        if (!IsLocked || bothLockedAndUnlocked)
        {
            if (_unlockedIcon) _unlockedIcon.sprite = spriteAtlas.GetSprite(spriteName);
        }
        return this;
    }
    public UniversalShopBlock SetIconColor(Color color, bool bothLockedAndUnlocked = false)
    {
        if (IsLocked || bothLockedAndUnlocked)
        {
            if (_lockedIcon) _lockedIcon.color = color;
        }
        if (!IsLocked || bothLockedAndUnlocked)
        {
            if (_unlockedIcon) _unlockedIcon.color = color;
        }
        return this;
    }
    public UniversalShopBlock SetIconVisible(bool state, bool bothLockedAndUnlocked = false)
    {
        if (IsLocked || bothLockedAndUnlocked)
        {
            if (_lockedIcon) _lockedIcon.gameObject.SetActive(state);
        }
        if (!IsLocked || bothLockedAndUnlocked)
        {
            if (_unlockedIcon) _unlockedIcon.gameObject.SetActive(state);
        }
        return this;
    }
    #endregion

    #region LOCK
    public UniversalShopBlock SetLocked(bool locked, bool withText = true)
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

    #region BUTTONS
    public UniversalShopBlock SetBuyButtonVisible(bool visible)
    {
        buyButton.gameObject.SetActive(visible);
        return this;
    }
    public UniversalShopBlock SetAdButtonVisible(bool visible)
    {
        buyAddButton.gameObject.SetActive(visible);
        return this;
    }
    #endregion

    #region BUTTONS HANDLERS
    public void OnBuyButtonClick()
    {
        if (!IsLocked)
        {
            if (buyButton)
            {
                if (!buyButton.IsLocked)
                {
                    onBuyButtonClicked?.Invoke(id);
                }
            }
            else
            {
                onBuyButtonClicked?.Invoke(id);
            }
        }
    }
    public void OnAdButtonClick()
    {
        if (!IsLocked)
        {
            if (buyAddButton)
            {
                if (!buyAddButton.IsLocked)
                {
                    onAdButtonClicked?.Invoke(id);
                }
            }
            else
            {
                onAdButtonClicked?.Invoke(id);
            }
        }
    }
    #endregion
}
