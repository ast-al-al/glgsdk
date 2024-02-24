using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "MainConfig", order = 56)]
public class MainConfig : ScriptableObject, IManagedService
{
    public GameConfig game;
    public CostsConfig costs;

    public Type Type => typeof(MainConfig);
}




#region GAME CONFIG
[System.Serializable]
public class CostsConfig
{

}
#endregion

#region COSTS CONFIG
[System.Serializable]
public class GameConfig
{

}
#endregion