using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GLG.UI
{
    public enum UIAnimationType
    {
        Instantly,
        LeftToRight,
        RighToLeft,
        UpToDown,
        DownToUp,
        ScaleWithOpacity,
        Opacity
    }

    public class GLGUI : MonoBehaviour, IManagedService
    {
        private Dictionary<Type, UIController> _UI = new Dictionary<Type, UIController>();

        public Type Type => typeof(GLGUI);

        public bool initializeOnStart = true;

        private void Awake()
        {
            if (initializeOnStart) Initialize();
        }

        public void Initialize()
        {
            foreach (Transform item in transform)
            {
                UIController controller;
                if (item.TryGetComponent(out controller))
                {
                    _UI.Add(controller.GetType(), controller);
                    controller.gameObject.SetActive(controller.immunityToHiding || controller.showOnStart);
                }
            }
        }

        public T Show<T>(System.Action callback = null) where T : UIController
        {
            UIController item = (T)_UI[typeof(T)];
            if (item.gameObject.activeSelf) return (T)item;
            item.Show(callback);
            return (T)item;
        }

        public T Show<T>(T controller, System.Action callback = null) where T : UIController
        {
            if (controller.gameObject.activeSelf) return (T)controller;
            controller.Show(callback);
            return controller;
        }

        public void HideAll()
        {
            foreach (var item in _UI)
            {
                item.Value.Hide();
            }
        }

        public T Hide<T>(System.Action callback = null) where T : UIController
        {
            UIController item = (T)_UI[typeof(T)];
            item.Hide(callback);
            return (T)item;
        }

        public void Get<T>(out T uiController) where T : UIController
        {
            foreach (var item in _UI)
            {
                if (item.Value is T value)
                {
                    uiController = value;
                    return;
                }
            }

            uiController = null;
        }

        public T Get<T>() where T : UIController => (T)_UI[typeof(T)];

        public void SetSortingOrderAsSiblingIndexes()
        {
            foreach (Transform item in transform)
            {
                item.GetComponent<Canvas>().sortingOrder = item.GetSiblingIndex();
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GLGUI))]
    public class GLGUIEditor : Editor
    {
        private GLGUI _target;

        private void OnEnable()
        {
            _target = target as GLGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set sorting order as sibling indexes"))
            {
                _target.SetSortingOrderAsSiblingIndexes();
            }
        }
    }
#endif
}