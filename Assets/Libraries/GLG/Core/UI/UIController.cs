using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

namespace GLG.UI
{
    /// <summary>
    /// Базовый класс для всех экранов и всплывающих окон.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIController : MonoBehaviour
    {
        [Header("===== UI Controller =========================================================")]
        public UIAnimationType defaultIntroAnimationType = UIAnimationType.ScaleWithOpacity;
        public UIAnimationType defaultOutroAnimationType = UIAnimationType.ScaleWithOpacity;
        public Ease ease = Ease.InOutQuint;
        public float defaultAnimationDuration = 0.3f;
        public bool setAsLastSiblingOnShow = true;
        public bool immunityToHiding = false;
        public bool showOnStart = false;
        public UnityEvent onShowed;
        public event Action onShowedE;
        public UnityEvent onHided;
        public event Action onHidedE;
        protected event Action _onShowedOnce;
        protected event Action _onHidedOnce;
        [SerializeField] private Dictionary<Type, UIController> _subcontrollers = new Dictionary<Type, UIController>();
        [SerializeField] protected Transform _container;
        [SerializeField] private CanvasGroup _canvasGroup;

        public bool IsShowing { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _container = transform.Find("Container");
            TryGetComponent(out _canvasGroup);
            _subcontrollers.Clear();
            UIController[] c = transform.GetComponentsInChildren<UIController>();
            foreach (var item in c)
            {
                _subcontrollers.Add(item.GetType(), item);
            }
        }
#endif
        public void Show()
        {
            Show(null);
        }
        public void Show(System.Action callback)
        {
            Show(defaultIntroAnimationType, defaultAnimationDuration, callback);
            onShowed.Invoke();
            onShowedE?.Invoke();
        }
        public void Show(UIAnimationType animationType, float animationDuration = 0f, System.Action callback = null)
        {
            OnStartShow();
            //if (!transform) transform = GetComponent<RectTransform>();
            if (setAsLastSiblingOnShow) _container.SetAsLastSibling();
            _container.localScale = Vector3.one;
            _container.localPosition = Vector3.zero;
            if (_canvasGroup == null) _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            switch (animationType)
            {
                case UIAnimationType.Instantly:
                    _canvasGroup.alpha = 1f;
                    gameObject.SetActive(true);
                    _container.localPosition = Vector3.zero;
                    OnEndShow();
                    if (callback != null) callback();
                    onShowed.Invoke();
                    onShowedE?.Invoke();
                    break;
                case UIAnimationType.LeftToRight:
                    _canvasGroup.alpha = 1f;
                    _container.localPosition = new Vector3(-Screen.width, 0f, 0f);
                    gameObject.SetActive(true);
                    _container.DOLocalMove(Vector3.zero, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); onShowed.Invoke(); onShowedE?.Invoke(); }).SetEase(ease);
                    break;
                case UIAnimationType.RighToLeft:
                    _canvasGroup.alpha = 1f;
                    _container.localPosition = new Vector3(Screen.width, 0f, 0f);
                    gameObject.SetActive(true);
                    _container.DOLocalMove(Vector3.zero, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); onShowed.Invoke(); onShowedE?.Invoke(); }).SetEase(ease);
                    break;
                case UIAnimationType.UpToDown:
                    _canvasGroup.alpha = 1f;
                    _container.localPosition = new Vector3(0f, Screen.height, 0f);
                    gameObject.SetActive(true);
                    _container.DOLocalMove(Vector3.zero, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); onShowed.Invoke(); onShowedE?.Invoke(); }).SetEase(ease);
                    break;
                case UIAnimationType.DownToUp:
                    _canvasGroup.alpha = 1f;
                    _container.localPosition = new Vector3(0f, -Screen.height, 0f);
                    gameObject.SetActive(true);
                    _container.DOLocalMove(Vector3.zero, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); onShowed.Invoke(); onShowedE?.Invoke(); }).SetEase(ease);
                    break;
                case UIAnimationType.ScaleWithOpacity:
                    _container.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    _canvasGroup.alpha = 0f;
                    gameObject.SetActive(true);
                    _container.DOScale(Vector3.one, animationDuration);
                    _canvasGroup.DOFade(1f, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); onShowed.Invoke(); onShowedE?.Invoke(); }).SetEase(ease);
                    break;
                case UIAnimationType.Opacity:
                    _container.localPosition = Vector3.zero;
                    _canvasGroup.alpha = 0f;
                    gameObject.SetActive(true);
                    _canvasGroup.DOFade(1f, animationDuration).OnComplete(() => { OnEndShow(); if (callback != null) callback(); }).SetEase(ease);
                    break;
                default:
                    break;
            }
            IsShowing = true;
        }
        public void Hide()
        {
            Hide(null);
        }
        public void Hide(System.Action callback)
        {
            Hide(defaultOutroAnimationType, defaultAnimationDuration, callback);
            onHided.Invoke();
            onHidedE?.Invoke();
        }
        public void Hide(UIAnimationType animationType, float animationDuration = 0f, System.Action callback = null)
        {
            if (!gameObject.activeSelf || immunityToHiding) return;
            OnStartHide();
            switch (animationType)
            {
                case UIAnimationType.Instantly:

                    gameObject.SetActive(false);
                    OnEndHide();
                    if (callback != null) callback();
                    break;
                case UIAnimationType.LeftToRight:
                    _container.DOLocalMove(new Vector3(Screen.width, 0f, 0f), animationDuration)
                        .OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        }).SetEase(ease);
                    break;
                case UIAnimationType.RighToLeft:
                    _container.DOLocalMove(new Vector3(-Screen.width, 0f, 0f), animationDuration)
                        .OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        }).SetEase(ease);
                    break;
                case UIAnimationType.UpToDown:
                    _container.DOLocalMove(new Vector3(0f, -Screen.height, 0f), animationDuration)
                        .OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        });
                    break;
                case UIAnimationType.DownToUp:
                    _container.DOLocalMove(new Vector3(0f, Screen.height, 0f), animationDuration)
                        .OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        }).SetEase(ease);
                    break;
                case UIAnimationType.ScaleWithOpacity:
                    if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    _canvasGroup.DOFade(0f, animationDuration).SetEase(ease);
                    _container.DOScale(new Vector3(0.9f, 0.9f, 0.9f), animationDuration).OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        }).SetEase(ease);
                    break;
                case UIAnimationType.Opacity:
                    if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    _canvasGroup.DOFade(0f, animationDuration)
                        .OnComplete(() =>
                        {
                            OnEndHide();
                            gameObject.SetActive(false);
                            if (callback != null) callback();
                        }).SetEase(ease);
                    break;
                default:
                    break;
            }
            IsShowing = false;
        }
        public T Get<T>() where T : UIController => (T)_subcontrollers[typeof(T)];
        
        public UIController OnShowed(System.Action callback)
        {
            _onShowedOnce += callback;
            return this;
        }
        public UIController OnHided(System.Action callback)
        {
            _onHidedOnce += callback;
            return this;
        }


        protected virtual void OnStartShow() { }
        protected virtual void OnEndShow() 
        {
            onShowed.Invoke();
            onShowedE?.Invoke();
            _onShowedOnce?.Invoke();
            _onShowedOnce = null;
        }
        protected virtual void OnStartHide() { }
        protected virtual void OnEndHide() 
        {
            onHided.Invoke();
            onHidedE?.Invoke();
            _onHidedOnce?.Invoke();
            _onHidedOnce = null;
        }
    }
}
