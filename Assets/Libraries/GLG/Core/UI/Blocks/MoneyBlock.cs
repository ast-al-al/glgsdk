using GLG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyValue;
    [SerializeField] private ContentSizeFitter _contentSizeFitter;
    [SerializeField] private LayoutGroup _layoutGroup;

    private int _money = 0;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            _moneyValue.text = _money.ToString();
        }
    }
    public Transform MoneyTransform => _moneyValue.transform;

    private void Awake()
    {
        PlayerMoney.OnMoneyValueChanged += MoneyChangedHandler;
        Money = Kernel.Economic.PlayerMoney.Money;
    }
    private void OnDestroy()
    {
        PlayerMoney.OnMoneyValueChanged -= MoneyChangedHandler;
    }

    private void MoneyChangedHandler(int value)
    {
        Money = value;
    }
    public void RefreshLayout()
    {
        Canvas.ForceUpdateCanvases();
        _contentSizeFitter.enabled = false;
        _contentSizeFitter.enabled = true;
        _layoutGroup.enabled = false;
        _layoutGroup.enabled = true;
    }
}
