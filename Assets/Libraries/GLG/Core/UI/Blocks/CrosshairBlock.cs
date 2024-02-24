using DG.Tweening;
using GLG;
using UnityEngine;

public class CrosshairBlock : MonoBehaviour, IManaged
{
    [SerializeField] private RectTransform _mainCrosshairContainer;
    [SerializeField] private Vector2 _defaultSize;
    [SerializeField] private Vector2 _defaultPushForce;
    [SerializeField] private Vector2 _maxPushDistance;
    [SerializeField] private float _returnSpeed;
    [Space]
    [SerializeField] private RectTransform _hitCrosshairContainer;
    [SerializeField] private CanvasGroup _hitContainerCanvasGroup;
    [SerializeField] private Vector2 _startHitContainerSize;
    [SerializeField] private Vector2 _endHitContainerSize;
    [SerializeField] private float _hitAnimationSpeed;
    [SerializeField] private AnimationCurve _hitSizeCurve;
    [SerializeField] private AnimationCurve _hitAlphaCurve;

    private Vector2 _currentMainCrosshairSize;
    private Tween _hitAlphaTween;
    private Tween _hitSizeTween; 
    private float _currentHitAnimationProgress = 1f;
    private Vector2 _currentHitContainerSize;

    private void Awake()
    {
        Kernel.RegisterManaged(this);
        _currentMainCrosshairSize = _defaultSize;
        _mainCrosshairContainer.sizeDelta = _currentMainCrosshairSize;
    }
    private void OnDestroy()
    {
        Kernel.UnregisterManaged(this);
    }

    public void ManagedUpdate()
    {
        _mainCrosshairContainer.sizeDelta = _currentMainCrosshairSize;
        _currentMainCrosshairSize = Vector2.MoveTowards(_currentMainCrosshairSize, _defaultSize, _returnSpeed * Time.deltaTime);
        _currentHitAnimationProgress = Mathf.MoveTowards(_currentHitAnimationProgress, 1f, _hitAnimationSpeed * Time.deltaTime);
        _currentHitContainerSize = Vector2.Lerp(_startHitContainerSize, _endHitContainerSize, _hitSizeCurve.Evaluate(_currentHitAnimationProgress));
        _hitCrosshairContainer.sizeDelta = _currentHitContainerSize;
        _hitContainerCanvasGroup.alpha = _hitAlphaCurve.Evaluate(_currentHitAnimationProgress);
    }



    public void DoPushIn(Vector2 pushForceMultiplier)
    {
        float x = Mathf.Clamp(_defaultPushForce.x * pushForceMultiplier.x, 0f, _maxPushDistance.x);
        float y = Mathf.Clamp(_defaultPushForce.y * pushForceMultiplier.y, 0f, _maxPushDistance.y);
        _currentMainCrosshairSize.x -= x;
        _currentMainCrosshairSize.y -= y;
    }
    public void DoPushOut(Vector2 pushForceMultiplier)
    {
        float x = _defaultPushForce.x * pushForceMultiplier.x;
        float y = _defaultPushForce.y * pushForceMultiplier.y;
        _currentMainCrosshairSize.x
            = Mathf.Clamp(_currentMainCrosshairSize.x + x, 0f, _maxPushDistance.x);
        _currentMainCrosshairSize.y
            = Mathf.Clamp(_currentMainCrosshairSize.y + y, 0f, _maxPushDistance.y);
    }

    public void DoHit()
    {
        _hitAlphaTween.Kill();
        _hitSizeTween.Kill();
        _currentHitAnimationProgress = 0f;
    }
}
