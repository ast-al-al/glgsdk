using System;
using System.Collections.Generic;
using UnityEngine;

public class Economic : MonoBehaviour, IManagedService, IManaged
{
    //[SerializeField] private PlayerShopsManager _playerShopsManager;
    private PlayerMoney _playerMoney;
    public PlayerMoney PlayerMoney => _playerMoney;
    //public PlayerShopsManager PlayerShopsManager => _playerShopsManager;

    public Type Type => typeof(Economic);

    private void Awake()
    {
        _playerMoney = new PlayerMoney();
    }
    /// <summary>
    /// @binding M - give money;
    /// </summary>
    public void ManagedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            _playerMoney.Money += 1000;
        }
#endif
        _playerMoney.ManagedUpdate();
    }
    private void OnDestroy()
    {
        _playerMoney.Save();
    }
    private void OnApplicationQuit()
    {
        _playerMoney.Save();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        _playerMoney.Save();
    }
}