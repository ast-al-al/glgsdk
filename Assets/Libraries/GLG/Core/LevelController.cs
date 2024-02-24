using GLG;
using UnityEngine;

public class LevelController : MonoBehaviour, IManaged
{
    private InGameScreen _ingameScreen;
    private LobbyOverlay _lobbyOverlay;

    #region UNITY CALLS
    private void Awake()
    {
        Kernel.RegisterManaged(this);
        _ingameScreen = Kernel.UI.Get<InGameScreen>();
        _lobbyOverlay = Kernel.UI.Get<LobbyOverlay>();
        
        AddHandlers();
        
    }
    private void OnDestroy()
    {
        Kernel.UnregisterManaged(this);
        RemoveHandlers();
    }
    #endregion

    #region PRIVATE METHODS
    private void AddHandlers()
    {
        _lobbyOverlay.OnStartButtonClick += GameStartedHandler;
    }
    private void RemoveHandlers()
    {
        _lobbyOverlay.OnStartButtonClick -= GameStartedHandler;
    }
    private void Win()
    {
        _ingameScreen.Hide();
        WinScreen winScreen = Kernel.UI.Show<WinScreen>();
        winScreen.LevelBlock.Value = Kernel.LevelsManager.DisplayLevelIndex.ToString();
        winScreen.OnNext(Kernel.LevelsManager.NextLevel);
        Kernel.UI.Hide<JoystickOverlay>().joystick.ForceStop();
    }
    private void Fail()
    {
        _ingameScreen.Hide();
        FailScreen failScreen = Kernel.UI.Show<FailScreen>();
        failScreen.LevelBlock.Value = Kernel.LevelsManager.DisplayLevelIndex.ToString();
        failScreen.OnNext(Kernel.LevelsManager.RestartLevel);
        Kernel.UI.Hide<JoystickOverlay>().joystick.ForceStop();
    }
    
    #endregion

    #region PUBLIC METHODS
    public void ManagedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fail();
        }
#endif
    }
    #endregion

    #region HANDLERS
    private void GameStartedHandler()
    {
        _lobbyOverlay.Hide();
        Kernel.UI.Show<InGameScreen>();
        Kernel.UI.Show<JoystickOverlay>();
    }
    #endregion
}