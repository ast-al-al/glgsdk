using DG.Tweening;
using GLG;
using GLG.UI;
using UnityEngine;
using UnityEngine.UI;

public class OfflineIncomeOverlay : UIController
{
    [SerializeField] private Vector2 _noThanksButtonStartPosition;
    [SerializeField] private Vector2 _windowStartSize;
    [SerializeField] private Vector2 _windowNormalSize;
    [SerializeField] private Text _adMultiplierText;
    [SerializeField] private Text _adEarnText;
    [SerializeField] private Text _earnValueText;
    [SerializeField] private Button _adButton;
    [SerializeField] private Button _noThanksButton;
    [SerializeField] private RectTransform _noThanksButtonTransform;
    [SerializeField] private RectTransform _window;

    private int _reward = 0;
    private float _rvMultiplier = 0;

    protected override void OnStartShow()
    {
        _adButton.interactable = true;
        _noThanksButton.interactable = true;
        _window.DOKill();
        _window.sizeDelta = _windowStartSize;
        _window.DOSizeDelta(_windowNormalSize, 1f).SetEase(Ease.InOutCirc);
        _noThanksButtonTransform.DOKill();
        _noThanksButtonTransform.anchoredPosition = _noThanksButtonStartPosition;
        _noThanksButtonTransform.DOAnchorPosY(0f, 1f).SetDelay(1f).SetEase(Ease.InOutCirc);
    }
    protected override void OnEndShow()
    {
        base.OnEndShow();
        Kernel.UI.Get<JoystickOverlay>().joystick.ForceStop();
    }

    public OfflineIncomeOverlay SetBaseReward(int money)
    {
        _reward = money;
        _earnValueText.text = '+' + money.ToString();
        _adEarnText.text = '+' + (_reward * _rvMultiplier).ToString("0.#");
        return this;
    }
    public OfflineIncomeOverlay SetRVMultiplier(float multiplier)
    {
        _rvMultiplier = multiplier;
        _adMultiplierText.text = 'x' + multiplier.ToString("0.#");
        _adEarnText.text = '+' + (_reward * _rvMultiplier).ToString("0.#");
        return this;
    }



    public void NoThanksButtonHandler()
    {
        Kernel.Economic.PlayerMoney.AddMoney(_reward);
        Hide();
    }
    public void AdButtonHandler() //TODO
    {
        /*
        if (AdWrapper.Rewarded.IsReady)
        {
            _adButton.interactable = false;
            _noThanksButton.interactable = false;
            GameParametersHub.IsWatchingAd = true;
            AdWrapper.Rewarded.ClearCallbacks().Show("reward_ingame_rvOfflineIncome")
                .OnOpened(() =>
                {
                })
                .OnSuccess(() =>
                {
                    Kernel.Economic.PlayerMoney.AddMoney((int)(_reward * _rvMultiplier) - _reward);
                })
                .OnClosed(() =>
                {
                    Kernel.Economic.PlayerMoney.AddMoney(_reward);
                    Hide();
                    GameParametersHub.IsWatchingAd = false;
                });
        }
        else
        {
            ShowNotify(I2.Loc.LocalizationManager.GetTranslation("notify_advNotReady"));
        }
        */
    }
    private void ShowNotify(string text)
    {
        Kernel.UI.Show<NotifyOverlay>().SetText(text);
    }
}
