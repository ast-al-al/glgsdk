using GLG.Ads;
using GLG.UI;
using GLG.Analytics;
using System;
using UnityEngine;

namespace GLG
{
    public class KernelLinker : MonoBehaviour
    {
        public event Action onUpdate;
        public event Action onFixedUpdate;
        public event Action onLateUpdate;

        [SerializeField] private bool _useDebugLog;
        [SerializeField] private bool _disableDebugLogInRelease;
        [SerializeField] private Transform _modulesParent;
        [SerializeField] private MonoBehaviour _objForCoroutines;
        [SerializeField] private Economic _economic;
        [SerializeField] private MainConfig _config;
        [SerializeField] private AdsWrapper _ads;
        [SerializeField] private GLGUI _ui;
        [SerializeField] private LevelsManager _levelManager;
        [SerializeField] private VFXFactory _vfxFactory;
        [SerializeField] private AnalyticsWrapper _analytics;
        [SerializeField] private CameraService _cameraService;

        public bool UseDebugLog => _useDebugLog;
        public bool DisableDebugLogInRelease => _disableDebugLogInRelease;
        public Transform ModulesParent { get => _modulesParent; }
        public MonoBehaviour ObjForCoroutines { get => _objForCoroutines; }
        public Economic Economic { get => _economic; }
        public MainConfig Config { get => _config; }
        public AdsWrapper Ads { get => _ads; }
        public GLGUI Ui { get => _ui; }
        public LevelsManager LevelManager { get => _levelManager; }
        public VFXFactory VfxFactory { get => _vfxFactory; }
        public AnalyticsWrapper Analytics { get => _analytics; }
        public CameraService CameraService { get => _cameraService; }


        #region UNITY MESSAGES
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Kernel.Initialize(this);
            RegisterModules();
        }
        private void Update()
        {
            onUpdate();
        }
        private void FixedUpdate()
        {
            onFixedUpdate();
        }
        private void LateUpdate()
        {
            onLateUpdate();
        }
        private void OnDestroy()
        {
            Kernel.Dispose();
        }
        #endregion

        private void RegisterModules()
        {
            MonoBehaviour[] objects = _modulesParent.GetComponentsInChildren<MonoBehaviour>();
            foreach (var item in objects)
            {
                if (item is IManagedService)
                {
                    Kernel.RegisterService(item);
                }
            }
        }
    }
}
