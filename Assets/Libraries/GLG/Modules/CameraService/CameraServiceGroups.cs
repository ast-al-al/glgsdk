using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraServiceGroups : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _groupsCameras;
    [SerializeField] private Transform _groupsParent;
    [SerializeField] private CinemachineTargetGroup _groupPrefab;
    [SerializeField] private CameraService _cameraService;
    
    private Dictionary<string, CinemachineTargetGroup> _groups = new Dictionary<string, CinemachineTargetGroup>();
    private int _currentCamera;
    private const string _camPrefix = "groups:cam";

    public void Start()
    {
        for (int i = 0; i < _groupsCameras.Length; i++)
        {
            _cameraService.RegisterTarget(_camPrefix + i, _groupsCameras[i]);
        }
    }

    public void CreateGroup(string key)
    {
        if (_groups.ContainsKey(key))
        {
            ClearGroup(key);
            return;
        }
        CinemachineTargetGroup groupInstance = Instantiate(_groupPrefab, _groupsParent);
        groupInstance.name = "group[" + key + "]";
        _groups[key] = groupInstance;
    }

    public void AddToGroup(string key, Transform target, float radius = 1f, float weight = 1f)
    {
        _groups[key].AddMember(target, weight, radius);
    }

    public void AddToGroup(string key, Transform[] targets, float radius = 1f, float weight = 1f)
    {
        CinemachineTargetGroup group = _groups[key];
        foreach (Transform target in targets)
        {
            group.AddMember(target, weight, radius);
        }
    }

    public void AddToGroup(string key, List<Transform> targets, float radius = 1f, float weight = 1f)
    {
        CinemachineTargetGroup group = _groups[key];
        foreach (Transform target in targets)
        {
            group.AddMember(target, weight, radius);
        }
    }

    public void RemoveFromGroup(string key, Transform target)
    {
        _groups[key].RemoveMember(target);
    }

    public void RemoveFromGroup(string key, Transform[] targets)
    {
        CinemachineTargetGroup group = _groups[key];
        foreach (Transform target in targets)
        {
            group.RemoveMember(target);
        }
    }

    public void RemoveFromGroup(string key, List<Transform> targets)
    {
        CinemachineTargetGroup group = _groups[key];
        foreach (Transform target in targets)
        {
            group.RemoveMember(target);
        }
    }

    public void ClearGroup(string key)
    {
        _groups[key].m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
    }

    public void RemoveGroup(string key)
    {
        if (_groups.ContainsKey(key))
        {
            Destroy(_groups[key].gameObject);
            _groups.Remove(key);
        }
    }

    public void SwitchToGroup(string key)
    {
        _currentCamera++;
        if (_currentCamera >= _groups.Count)
        {
            _currentCamera = 0;
        }
        _groupsCameras[_currentCamera].Follow = _groups[key].transform;
        _groupsCameras[_currentCamera].LookAt = _groups[key].transform;
        _cameraService.SwitchTarget(_camPrefix + _currentCamera);
    }
}
