using GLG.UI;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : UIController
{
    public event System.Action onNext;

    [SerializeField] private Button _replayButton;
    [SerializeField] private TextBlock _levelBlock;

    public TextBlock LevelBlock => _levelBlock;

    private void Awake()
    {
        _replayButton.onClick.AddListener(ReplayButtonHandler);
    }
    private void OnDestroy()
    {
        _replayButton.onClick.RemoveListener(ReplayButtonHandler);
    }

    public FailScreen OnNext(System.Action callback)
    {
        onNext += callback;
        return this;
    }

    private void ReplayButtonHandler()
    {
        onNext?.Invoke();
        onNext = null;
    }
}
