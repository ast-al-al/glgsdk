using TMPro;
using UnityEngine;

public class TextBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _value;

    public string Text
    {
        get => _text.text;
        set => _text.text = value;
    }
    public string Value
    {
        get => _value.text;
        set => _value.text = value;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(!_text) transform.GetChild(0).TryGetComponent(out _text);
        if (!_value) transform.GetChild(1).TryGetComponent(out _value);
    }
#endif
}
