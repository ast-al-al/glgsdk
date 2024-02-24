using GLG.UI;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : UIController
{
    private event System.Action _onNext;

    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private TextBlock _levelBlock;

    public TextBlock LevelBlock => _levelBlock;

    private void Awake()
    {
        _nextLevelButton.onClick.AddListener(NextLevelButtonHandler);
    }
    private void OnDestroy()
    {
        _nextLevelButton.onClick.RemoveListener(NextLevelButtonHandler);
        _onNext = null;
    }

    public WinScreen OnNext(System.Action callback)
    {
        _onNext += callback;
        return this;
    }

    public void NextLevelButtonHandler()
    {
        _onNext?.Invoke();
        _onNext = null;
    }
}
