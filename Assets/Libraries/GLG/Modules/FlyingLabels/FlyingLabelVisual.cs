using TMPro;
using UnityEngine;

public class FlyingLabelVisual : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _text;

    public RectTransform CashedTransform => _rectTransform;

    public FlyingLabelVisual SetText(string text)
    {
        _text.text = text;
        return this;
    }
}
