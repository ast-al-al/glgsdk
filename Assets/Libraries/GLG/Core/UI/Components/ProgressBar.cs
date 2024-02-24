using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _filling;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _cashedTransform;

    public RectTransform CashedTransform => _cashedTransform;
    public CanvasGroup CanvasGroup => _canvasGroup;

    public float Value { get => _filling.fillAmount; set => _filling.fillAmount = value; }

    public void SetValue(float value)
    {
        _filling.fillAmount = value;
    }
}
