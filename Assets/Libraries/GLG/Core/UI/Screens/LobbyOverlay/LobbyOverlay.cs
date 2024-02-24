using GLG.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOverlay : UIController
{
    public event System.Action OnStartButtonClick;

    [Header("Lobby Overlay")]
    [SerializeField] private Button _startButton;
    [SerializeField] private bool _autoCloseOnStart = true;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private string _levelTextPrefix;

    private int _level;

    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            _levelText.text = _levelTextPrefix + value;
        }
    }

    protected override void OnStartShow()
    {
        base.OnStartShow();
        _startButton.onClick.AddListener(StartButtonHandler);
    }
    protected override void OnStartHide()
    {
        base.OnStartHide();
        _startButton.onClick.RemoveListener(StartButtonHandler);
    }

    private void StartButtonHandler()
    {
        if(_autoCloseOnStart)
        {
            Hide();
        }
        OnStartButtonClick?.Invoke();
    }
}
