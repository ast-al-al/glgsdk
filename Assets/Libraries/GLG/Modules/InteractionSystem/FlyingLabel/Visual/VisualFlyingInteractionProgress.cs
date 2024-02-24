using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualFlyingInteractionProgress : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _objForScale;
    [SerializeField] private Image _imgForFilling;
    [SerializeField] private TMP_Text _txtForProgress;
    [SerializeField] private RectTransform _visualRoot;
    [Header("Text settings")]
    [SerializeField] private string _txtPrefix;
    [SerializeField] private string _txtPostfix;
    [Header("Animation settings")]
    [SerializeField] private float _introAnimationDuration;
    [SerializeField] private float _outroAnimationDuration;
    [SerializeField] private Ease _introEase;
    [SerializeField] private Ease _outroEase;

    private bool _useScale;
    private bool _useFilling;
    private bool _useText;
    private Tween _movingTween;

    public RectTransform RectTransform => _rectTransform;
    public RectTransform VisualRoot => _visualRoot;

    private void Awake()
    {
        if (_introAnimationDuration > 0f)
        {
            _visualRoot.localScale = new Vector3(0.001f, 0.001f, 1f);
        }
        _useScale = _objForScale != null;
        _useFilling = _imgForFilling != null;
        _useText = _txtForProgress != null;
        if (_useScale)
        {
            _objForScale.localScale = new Vector3(0.001f, 0.001f, 1f);
        }
        if (_useFilling)
        {
            _imgForFilling.fillAmount = 0f;
        }
        if (_useText)
        {
            _txtForProgress.text = _txtPrefix + '0' + _txtPostfix;
        }
    }

    public VisualFlyingInteractionProgress SetProgress(float value)
    {
        if (_useScale)
        {
            float scale = value + 0.001f;
            _objForScale.localScale = new Vector3(scale, scale, 1f);
        }
        if (_useFilling)
        {
            _imgForFilling.fillAmount = value;
        }
        if (_useText)
        {
            _txtForProgress.text = _txtPrefix + (int)(value * 100f) + _txtPostfix;
        }
        return this;
    }

    public VisualFlyingInteractionProgress Show(System.Action callback = null)
    {
        gameObject.SetActive(true);
        if (_introAnimationDuration > 0f)
        {
            _visualRoot.DOKill();
            SmoothResetVisualPosition();
            _visualRoot.DOScale(1f, _introAnimationDuration)
                .SetEase(_introEase)
                .OnComplete(() => { callback?.Invoke(); });
        }
        else
        {
            callback?.Invoke();
        }
        return this;
    }
    public VisualFlyingInteractionProgress Hide(System.Action callback = null)
    {
        if (_outroAnimationDuration > 0f)
        {
            _visualRoot.DOKill();
            _visualRoot.DOScale(0.001f, _outroAnimationDuration)
                .SetEase(_outroEase)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    Destroy(gameObject);
                });
        }
        else
        {
            gameObject?.SetActive(false);
            callback?.Invoke();
        }
        return this;
    }
    public void SmoothResetVisualPosition()
    {
        if (_movingTween != null)
        {
            _movingTween.Kill();
        }
        _movingTween = _visualRoot.DOLocalMove(Vector3.zero, _introAnimationDuration);
    }
}
