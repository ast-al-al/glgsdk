using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraService : MonoBehaviour, IManagedService
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CameraServiceGroups _groupsController;
    
    public Type Type => typeof(CameraService);
    public Camera MainCamera => _mainCamera;
    public CameraServiceGroups Groups => _groupsController;
    
    private Dictionary<string, CinemachineVirtualCamera> _targets = new Dictionary<string, CinemachineVirtualCamera>();
    
    public void RegisterTarget(string key, CinemachineVirtualCamera virtualCamera)
    {
        _targets[key] = virtualCamera;
    }
    public void UnregisterTarget(string key)
    {
        _targets.Remove(key);
    }

    public void SwitchTarget(string key)
    {
        if (_targets.ContainsKey(key))
        {
            foreach (var target in _targets)
            {
                target.Value.Priority = 0;
            }
            _targets[key].Priority = 1000;
        }
    }
}