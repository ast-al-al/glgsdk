using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string sceneName;
}


[CreateAssetMenu(fileName = "Levels preset", menuName = "GLG/Levels preset", order = 0)]
public class LevelsPreset : ScriptableObject
{
    public LevelData[] levels;
}
