using Cinemachine;
using GLG;
using UnityEngine;

public class CameraServiceTester : MonoBehaviour, IManaged
{
    [SerializeField] private CinemachineVirtualCamera _staticObj1;
    [SerializeField] private CinemachineVirtualCamera _staticObj2;
    [SerializeField] private Transform _dynamicObj1;
    [SerializeField] private Transform _dynamicObj2;
    
    private CameraService _cameraService;
    
    private void Start()
    {
        Kernel.RegisterManaged(this);
        _cameraService = Kernel.CameraService;
        _cameraService.RegisterTarget("test1", _staticObj1);
        _cameraService.RegisterTarget("test2", _staticObj2);
    }
    public void ManagedUpdate()
    {
        _dynamicObj1.transform.position = new Vector3(Mathf.Sin(Kernel.Time) * 5f, 0f, 0f);
        _dynamicObj2.transform.position = new Vector3(0f, 0f, Mathf.Sin(Kernel.Time) * 5f);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _cameraService.SwitchTarget("test1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _cameraService.SwitchTarget("test2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _cameraService.Groups.RemoveGroup("test4");
            
            _cameraService.Groups.CreateGroup("test3");
            _cameraService.Groups.AddToGroup("test3", _dynamicObj1);
            _cameraService.Groups.AddToGroup("test3", _dynamicObj2);
            _cameraService.Groups.SwitchToGroup("test3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _cameraService.Groups.RemoveGroup("test3");
            
            _cameraService.Groups.CreateGroup("test4");
            _cameraService.Groups.AddToGroup("test4", _staticObj1.transform);
            _cameraService.Groups.AddToGroup("test4", _staticObj2.transform);
            _cameraService.Groups.AddToGroup("test4", _dynamicObj2);
            _cameraService.Groups.SwitchToGroup("test4");
        }
    }
}