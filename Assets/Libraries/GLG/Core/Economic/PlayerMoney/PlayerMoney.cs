using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMoney : IManaged
{
    private int _money;
    public static event Action<int> OnMoneyValueChanged = (a) => { };
    public static event Action<MoneyKind, int> OnOtherMoneyValueChanged = (a, b) => { };
    private Dictionary<MoneyKind, int> _otherMoney = new Dictionary<MoneyKind, int>();
    private float _timeToNextSaveMoney;
    private bool _needSave;
    private int _startMoney;
    private List<MoneyKind> _otherMoneyToSave = new List<MoneyKind>();

    public int Money
    {
        get => _money;
        set => SetMoney(value);
    }

    public PlayerMoney()
    {
        _startMoney = PlayerPrefs.GetInt("Player_money", _startMoney);
        Money = _startMoney;
        OnMoneyValueChanged?.Invoke(_startMoney);
    }
    public void ManagedUpdate()
    {
        if (_needSave && Time.time > _timeToNextSaveMoney)
        {
            Save();
        }
    }

    /// <summary>
    /// Добавляет основную валюту. Оптимизированная функция без проверок. Не рекомендуется передавать отрицательные значения.
    /// </summary>
    /// <param name="value">Количество валюты для добавления</param>
    public void AddMoney(int value)
    {
        SetMoney(_money + value);
    }
    /// <summary>
    /// Универсальный метод взаимодействия с основной валютой. Принимает любые значения.
    /// </summary>
    /// <param name="value">Движение валюты</param>
    /// <returns>true - если у игрока достаточно валюты или баланс не был изменен, false - если не достаточно</returns>
    public bool InteractWithMoney(int value)
    {
        if (value == 0) return true;
        int newMoneyValue = _money + value;
        if (newMoneyValue >= 0)
        {
            SetMoney(newMoneyValue);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Устанавливает количество основной валюты игрока.
    /// </summary>
    /// <param name="value">Новое количество</param>
    public void SetMoney(int value)
    {
        _money = value;
        _timeToNextSaveMoney = Time.time + 1f;
        _needSave = true;
        OnMoneyValueChanged.Invoke(_money);
    }
    /// <summary>
    /// Добавляет дополнительную валюту. Оптимизированная функция без проверок. Не рекомендуется передавать отрицательные значения.
    /// </summary>
    /// <param name="key">Тип валюты</param>
    /// <param name="value">Количество валюты для добавления</param>
    public void AddOtherMoney(MoneyKind key, int value)
    {
        SetOtherMoney(key, _otherMoney[key] + value);
    }
    /// <summary>
    /// Универсальный метод взаимодействия с дополнительной валютой. Принимает любые значения.
    /// </summary>
    /// <param name="key">Тип валюты</param>
    /// <param name="value">Движение валюты</param>
    /// <returns>true - если у игрока достаточно валюты или баланс не был изменен, false - если не достаточно</returns>
    public bool InteractWithOtherMoney(MoneyKind key, int value)
    {
        if (value == 0) return true;
        int newMoneyValue = _money + _otherMoney[key];
        if (newMoneyValue >= 0)
        {
            SetOtherMoney(key, newMoneyValue);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Устанавливает количество дополнительной валюты.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetOtherMoney(MoneyKind key, int value)
    {
        _otherMoney[key] = value;
        if (!_otherMoneyToSave.Contains(key)) _otherMoneyToSave.Add(key);
        _timeToNextSaveMoney = Time.time + 1f;
        _needSave = true;
        OnOtherMoneyValueChanged(key, _otherMoney[key]);
    }
    /// <summary>
    /// Принудительное сохранение.
    /// </summary>
    public void Save()
    {
        _needSave = false;
        PlayerPrefs.SetInt("Player_money", Money);
        foreach (var item in _otherMoneyToSave)
        {
            PlayerPrefs.SetInt(item.ToString(), Money);
        }
        PlayerPrefs.Save();
    }
}