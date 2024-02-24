using GLG.Ads;
using GLG.Analytics;
using GLG.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GLG
{
    public static class Kernel
    {
        private static event Action OnUpdate = () => { };
        private static event Action OnFixedUpdate = () => { };
        private static event Action OnLateUpdate = () => { };

        public static bool UseDebug { get; set; }

        #region GETTERS

        public static float Time { get; private set; }

        public static float DeltaTime { get; private set; }

        // Объект для запуска независимых корутин.
        public static MonoBehaviour CoroutinesObject { get; private set; }

        // Базовые сервисы выведены в свойства для ускорения доступа.
        public static MainConfig Config { get; private set; }
        public static Economic Economic { get; private set; }
        public static AdsWrapper Ads { get; private set; }
        public static GLGUI UI { get; private set; }
        public static LevelsManager LevelsManager { get; private set; }
        public static VFXFactory VFXFactory { get; private set; }
        public static AnalyticsWrapper Analytics { get; private set; }
        public static Inventory Inventory { get; private set; }
        public static CameraService CameraService { get; private set; }
        public static LevelContext LevelContext { get; private set; }

        #endregion

        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        #region PUBLIC METHODS

        public static void Initialize(KernelLinker linker)
        {
            linker.onUpdate += UpdateHandler;
            linker.onFixedUpdate += FixedUpdateHandler;
            linker.onLateUpdate += LateUpdateHandler;
            Config = linker.Config;
            Inventory = new Inventory().Load("mi_"); // MainInventory
            Economic = linker.Economic;
            CoroutinesObject = linker.ObjForCoroutines;
            Ads = linker.Ads;
            UI = linker.Ui;
            LevelsManager = linker.LevelManager;
            VFXFactory = linker.VfxFactory;
            Analytics = linker.Analytics;
            CameraService = linker.CameraService;

            UseDebug = linker.UseDebugLog;

#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            if (linker.DisableDebugLogInRelease)
            {
                UseDebug = false;
            }
#endif


            if (Config) RegisterService(Config);
            if (Economic) RegisterService(Economic);
            if (Ads) RegisterService(Ads);
            if (UI) RegisterService(UI);
            if (LevelsManager) RegisterService(LevelsManager);
            if (VFXFactory) RegisterService(VFXFactory);
            if (Analytics) RegisterService(Analytics);
            RegisterService(Inventory);
            RegisterService(CameraService);

            Log("<color=green>Kernel initialized!</color>");
        }

        /// <summary>
        /// Подписывает объект на системные сообщения Update, Fixed Update, Late Update.
        /// Вызывает метод ManagedAwake, если объект реализует IManagedAwake.
        /// </summary>
        /// <param name="obj"></param>
        public static void RegisterManaged(object objInstance)
        {
            if (objInstance is IManaged imanaged)
            {
                OnUpdate += imanaged.ManagedUpdate;
            }

            if (objInstance is IManagedFixed ifixed)
            {
                OnFixedUpdate += ifixed.ManagedFixedUpdate;
            }

            if (objInstance is IManagedLate ilate)
            {
                OnLateUpdate += ilate.ManagedLateUpdate;
            }

            if (objInstance is IManagedAwake iawake)
            {
                iawake.ManagedAwake();
            }
        }

        /// <summary>
        /// Отписывает объект от системных сообщений Update, Fixed Update, Late Update.
        /// </summary>
        public static void UnregisterManaged(object objInstance)
        {
            if (objInstance is IManaged imanaged)
            {
                OnUpdate -= imanaged.ManagedUpdate;
            }

            if (objInstance is IManagedFixed ifixed)
            {
                OnFixedUpdate -= ifixed.ManagedFixedUpdate;
            }

            if (objInstance is IManagedLate ilate)
            {
                OnLateUpdate -= ilate.ManagedLateUpdate;
            }
        }

        /// <summary>
        /// Регистрирует сервис в системе. Подписывает на все Managed события.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterService<T>(T serviceInstance) where T : IManagedService
        {
            _services[serviceInstance.Type] = serviceInstance;
            RegisterManaged(serviceInstance);
        }

        public static void RegisterService(MonoBehaviour serviceInstance)
        {
            IManagedService service = serviceInstance as IManagedService;
            _services[service.Type] = service;
            RegisterManaged(service);
        }

        /// <summary>
        /// Удаляет сервис из системы. Отписывает его от Managed событий.
        /// </summary>
        public static void UnregisterService(IManagedService serviceInstance)
        {
            _services.Remove(serviceInstance.Type);
            UnregisterManaged(serviceInstance);
        }

        /// <summary>
        /// Создает объект сервиса. Не рекомендуется передавать тип, унаследованный от MonoBehaviour и ScriptableObject.
        /// </summary>
        /// <typeparam name="T">Тип создаваемого сервиса</typeparam>
        public static void CreateService<T>() where T : IManagedService, new()
        {
            T service = new T();
            RegisterService(service);
        }

        /// <summary>
        /// Возвращает сервис, если он зарегистрирован в системе.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : IManagedService
        {
            if (_services.TryGetValue(typeof(T), out object result))
            {
                return (T)result;
            }

            return default;
        }

        [HideInCallstack]
        public static void Log(string message, GameObject context = null, bool showContextName = true)
        {
            if (UseDebug)
            {
                if (showContextName && context)
                {
                    Debug.Log($"[{context.name}] {message}", context);
                }
                else
                {
                    Debug.Log(message, context);
                }
            }
        }

        [HideInCallstack]
        public static void Log(string sender, string message)
        {
            if (UseDebug)
            {
                Debug.Log($"[{sender}] {message}");
            }
        }

        [HideInCallstack]
        public static void Log(string message)
        {
            if (UseDebug)
            {
                Debug.Log(message);
            }
        }

        public static void Dispose()
        {
            OnUpdate = null;
            OnFixedUpdate = null;
            OnLateUpdate = null;
        }

        public static void UpdateLevelContext(LevelContext newLevelContext)
        {
            LevelContext = newLevelContext;
        }

        #endregion

        #region HANDLERS

        private static void UpdateHandler()
        {
            Time = UnityEngine.Time.time;
            DeltaTime = UnityEngine.Time.deltaTime;
            OnUpdate();
        }

        private static void FixedUpdateHandler()
        {
            OnFixedUpdate();
        }

        private static void LateUpdateHandler()
        {
            OnLateUpdate();
        }

        #endregion
    }
}