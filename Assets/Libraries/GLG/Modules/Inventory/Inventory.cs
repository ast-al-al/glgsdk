using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory : IManagedService
{
    private string _inventoryIdentifier;
    private Dictionary<EntityKind, Dictionary<string, int>> _inventories;
    private readonly StringBuilder _sb = new StringBuilder();
    private Dictionary<EntityKind, System.Action<string, int>> _inventoryEvents;
    private Dictionary<EntityKind, bool> _inventoryChanged;

    public Type Type => typeof(Inventory);

    #region SAVE / LOAD
    /// <summary>
    /// Загружает инвентарь из сохранения.
    /// </summary>
    public Inventory Load(string inventoryIdentifier)
    {
        _inventoryIdentifier = inventoryIdentifier;
        EntityKind[] allkinds = EntityData.GetKinds();
        _inventories = new Dictionary<EntityKind, Dictionary<string, int>>();
        _inventoryEvents = new Dictionary<EntityKind, System.Action<string, int>>(allkinds.Length);
        _inventoryChanged = new Dictionary<EntityKind, bool>(allkinds.Length);
        for (int i = 0; i < allkinds.Length; i++)
        {
            _sb.Clear();
            _sb.Append(this._inventoryIdentifier);
            _sb.Append(allkinds[i]);
            string data = PlayerPrefs.GetString(_sb.ToString());
            Dictionary<string, int> inventory = ParseItems(data);
            //Debug.Log(($"LOAD: {allkinds[i]} KEY:{_sb.ToString()}  DATA:{data}  INVENTORY:{inventory}"));
            if (inventory != null)
            {
                _inventories.Add(allkinds[i], inventory);
            }
            _inventoryEvents.Add(allkinds[i], null);
            _inventoryChanged.Add(allkinds[i], false);
        }
        return this;
    }
    /// <summary>
    /// Сохраняет инвентарь.
    /// </summary>
    public void Save()
    {
        foreach (var inventory in _inventories)
        {
            if (inventory.Value.Count > 0 && _inventoryChanged[inventory.Key])
            {
                _inventoryChanged[inventory.Key] = false;
                _sb.Clear();
                _sb.Append(_inventoryIdentifier);
                _sb.Append(inventory.Key);
                string key = _sb.ToString();
                int counter = 0;
                _sb.Clear();
                foreach (var item in inventory.Value)
                {
                    if (counter != 0) _sb.Append('|');
                    _sb.Append(item.Key);
                    _sb.Append(':');
                    _sb.Append(item.Value);
                    counter++;
                }
                PlayerPrefs.SetString(key, _sb.ToString());
            }
        }
    }
    #endregion

    #region GET
    /// <summary>
    /// Получает количество указанных предметов.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="universalKey">Идентификатор предмета</param>
    /// <returns>Количество предметов</returns>
    public int GetItems(EntityKind entityKind, string universalKey)
    {
        int result = 0;
        if (_inventories.TryGetValue(entityKind, out Dictionary<string, int> inventory))
        {
            inventory.TryGetValue(universalKey, out result);
        }
        return result;
    }
    /// <summary>
    /// Получает количество указанных предметов.
    /// </summary>
    /// <param name="entityData">Идентификационные данные предмета</param>
    /// <returns>Количество предметов</returns>
    public int GetItems(EntityData entityData)
    {
        return GetItems(entityData.entityKind, entityData.universalKey);
    }
    /// <summary>
    /// Получает список предметов указанного типа.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <returns>Список предметов указанного типа</returns>
    public Dictionary<string, int> GetInventory(EntityKind entityKind)
    {
        if (_inventories.TryGetValue(entityKind, out Dictionary<string, int> inventory))
        {
            return inventory;
        }
        return new Dictionary<string, int>(0);
    }
    #endregion

    #region ADD
    /// <summary>
    /// Добавляет указанное количество предметов в инвентарь.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="universalKey">Идентификатор предмета</param>
    /// <param name="value">Количеситво предметов для добавления</param>
    /// <returns>Количество предметов после операции</returns>
    public int AddItems(EntityKind entityKind, string universalKey, int value)
    {
        int newValue;
        if (_inventories.ContainsKey(entityKind))
        {
            Dictionary<string, int> inventory = _inventories[entityKind];
            if (inventory.ContainsKey(universalKey))
            {
                newValue = inventory[universalKey] + value;
                if (newValue <= 0)
                {
                    inventory.Remove(universalKey);
                    InventoryChangedHandler(entityKind, universalKey, 0);
                    return 0;
                }
                else
                {
                    inventory[universalKey] = newValue;
                    InventoryChangedHandler(entityKind, universalKey, newValue);
                    return newValue;
                }
            }
            else
            {
                if (value > 0)
                {
                    inventory.Add(universalKey, value);
                    InventoryChangedHandler(entityKind, universalKey, value);
                    return value;
                }
                else
                {
                    InventoryChangedHandler(entityKind, universalKey, 0);
                    return 0;
                }
            }
        }
        else
        {
            if (value > 0)
            {
                _inventories.Add(entityKind, new Dictionary<string, int>() { { universalKey, value } });
                InventoryChangedHandler(entityKind, universalKey, value);
                return value;
            }
            else
            {
                InventoryChangedHandler(entityKind, universalKey, 0);
                return 0;
            }
        }
    }
    /// <summary>
    /// Добавляет указанное количество предметов в инвентарь.
    /// </summary>
    /// <param name="entityData">Идентификационные данные предмета</param>
    /// <param name="value">Количеситво предметов для добавления</param>
    /// <returns>Количество предметов после операции</returns>
    public int AddItems(EntityData entityData, int value)
    {
        return AddItems(entityData.entityKind, entityData.universalKey, value);
    }
    /// <summary>
    /// Добавляет указанное количество предметов в инвентарь, если пользователь успешно просмотрел Rewarded Video.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="universalKey">Идентификатор предмета</param>
    /// <param name="value">Количеситво предметов для добавления</param>
    /// <param name="placement">Место показа рекламы для аналитики</param>
    /// <param name="onClosed">Обратный вызов при закрытии рекламы</param>
    /// <param name="onSuccess">Обратный вызов при успешном получении награды за Rewarded Video</param>
    /// <returns>true если реклама начала показываться, false если нет</returns>
    #endregion

    #region REMOVE
    /// <summary>
    /// Удаляет указанное количество предметов из инвентаря.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="universalKey">Идентификатор предмета</param>
    /// <param name="value"></param>
    /// <returns>Количество предметов после операции</returns>
    public int RemoveItems(EntityKind entityKind, string universalKey, int value)
    {
        return AddItems(entityKind, universalKey, -value);
    }
    /// <summary>
    /// Удаляет указанное количество предметов из инвентаря.
    /// </summary>
    /// <param name="entityData">Идентификационные данные предмета</param>
    /// <param name="value"></param>
    /// <returns>Количество предметов после операции</returns>
    public int RemoveItems(EntityData entityData, int value)
    {
        return RemoveItems(entityData.entityKind, entityData.universalKey, value);
    }
    #endregion

    #region SET
    /// <summary>
    /// Устанавливает указанное количество предметов в инвентаре.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="universalKey">Идентификатор предмета</param>
    /// <param name="value"></param>
    /// <returns>Количество предметов после операции</returns>
    public int SetItems(EntityKind entityKind, string universalKey, int value)
    {
        if (_inventories.ContainsKey(entityKind))
        {
            Dictionary<string, int> inventory = _inventories[entityKind];
            if (inventory.ContainsKey(universalKey))
            {
                if (value <= 0)
                {
                    inventory.Remove(universalKey);
                    InventoryChangedHandler(entityKind, universalKey, 0);
                    return 0;
                }
                else
                {
                    inventory[universalKey] = value;
                    InventoryChangedHandler(entityKind, universalKey, value);
                    return value;
                }
            }
            else
            {
                if (value > 0)
                {
                    inventory.Add(universalKey, value);
                    InventoryChangedHandler(entityKind, universalKey, value);
                    return value;
                }
                else
                {
                    InventoryChangedHandler(entityKind, universalKey, 0);
                    return 0;
                }
            }
        }
        else
        {
            if (value > 0)
            {
                _inventories.Add(entityKind, new Dictionary<string, int>() { { universalKey, value } });
                InventoryChangedHandler(entityKind, universalKey, value);
                return value;
            }
            else
            {
                InventoryChangedHandler(entityKind, universalKey, 0);
                return 0;
            }
        }
    }
    /// <summary>
    /// Устанавливает указанное количество предметов в инвентаре.
    /// </summary>
    /// <param name="entityData">Идентификационные данные предмета</param>
    /// <param name="value"></param>
    /// <returns>Количество предметов после операции</returns>
    public int SetItems(EntityData entityData, int value)
    {
        return SetItems(entityData.entityKind, entityData.universalKey, value);
    }
    #endregion

    #region EVENTS
    /// <summary>
    /// Подписывается на событие изменения определенного типа предметов в инвентаре.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="callback">Обратный вызов. 1 параметр - идентификатор предмета, 2 параметр - количество предметов после операции</param>
    public void SubscribeToInventoryChanges(EntityKind entityKind, System.Action<string, int> callback)
    {
        _inventoryEvents[entityKind] += callback;
    }
    /// <summary>
    /// Отписывается от события изменения определенного типа предметов в инвентаре.
    /// </summary>
    /// <param name="entityKind">Тип предмета</param>
    /// <param name="callback">Обратный вызов. 1 параметр - идентификатор предмета, 2 параметр - количество предметов после операции</param>
    public void UnsubscribeFromInventoryChanges(EntityKind entityKind, System.Action<string, int> callback)
    {
        _inventoryEvents[entityKind] -= callback;
    }
    #endregion

    #region PRIVATE METHODS
    private Dictionary<string, int> ParseItems(string data)
    {
        if (string.IsNullOrEmpty(data)) return null;
        string[] splittedData = data.Split('|');
        if (splittedData.Length == 0) return null;
        Dictionary<string, int> items = new Dictionary<string, int>(splittedData.Length);
        foreach (var item in splittedData)
        {
            string[] splittedItem = item.Split(':');
            items.Add(splittedItem[0], int.Parse(splittedItem[1]));
        }
        return items;
    }
    private void InventoryChangedHandler(EntityKind entityKind, string universalKey, int quantity)
    {
        _inventoryChanged[entityKind] = true;
        _inventoryEvents[entityKind]?.Invoke(universalKey, quantity);
    }
    #endregion
}