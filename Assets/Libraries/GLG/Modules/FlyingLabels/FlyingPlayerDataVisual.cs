using TMPro;
using UnityEngine;

public class FlyingPlayerDataVisual : MonoBehaviour
{
    [SerializeField] private RectTransform _cashedRectTransform;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private TMP_Text _nickText;
    [SerializeField] private TMP_Text _hpText;

    public ProgressBar ProgressBar => _progressBar;
    public TMP_Text NickText => _nickText;
    public TMP_Text HPText => _hpText;
    public RectTransform CashedTransform => _cashedRectTransform;

    public FlyingPlayerDataVisual SetNick(string nick)
    {
        _nickText.text = nick;
        return this;
    }
    public FlyingPlayerDataVisual SetHp(string text)
    {
        _hpText.text = text;
        return this;
    }
    public FlyingPlayerDataVisual SetProgressBarValue(float value)
    {
        _progressBar.Value = value;
        return this;
    }
}
