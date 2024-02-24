using GLG;
using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour, IManagedService
{
    [SerializeField, Expandable] private LevelsPreset _levelsPreset;
    public int LevelIndex { get; private set; } = -1;
    public int DisplayLevelIndex { get; private set; } = 0;
    public Type Type => typeof(LevelsManager);

    private string _currentScene;

    private void Start()
    {
        LevelIndex = PlayerPrefs.GetInt("levelIndex", 0) - 1;
        DisplayLevelIndex = PlayerPrefs.GetInt("displayLevelIndex", 1) - 1;
        NextLevel();
    }
    public void NextLevel()
    {
        LevelIndex++;
        DisplayLevelIndex++;
        if (LevelIndex > _levelsPreset.levels.Length - 1)
        {
            LevelIndex = 0;
        }
        PlayerPrefs.SetInt("levelIndex", LevelIndex);
        PlayerPrefs.SetInt("displayLevelIndex", DisplayLevelIndex);
        StartCoroutine(LoadScene(_levelsPreset.levels[LevelIndex].sceneName));
        Kernel.UI.Get<LobbyOverlay>().Level = DisplayLevelIndex;
    }
    public void RestartLevel()
    {
        StartCoroutine(LoadScene(_currentScene));
    }



    IEnumerator LoadScene(string name)
    {
        Kernel.CameraService.MainCamera.transform.SetParent(Kernel.UI.transform);
        Kernel.UI.HideAll();
        Kernel.UI.Show<LoadingOverlay>();
        if (!string.IsNullOrEmpty(_currentScene))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_currentScene);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
        _currentScene = name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        yield return new WaitForSeconds(0.5f);
        Kernel.UI.Hide<LoadingOverlay>();
        Kernel.UI.Show<LobbyOverlay>();
    }
}
