using System.Collections.Generic;
using UnityEngine;

public class EntitiesRegistry : MonoBehaviour
{

    private static bool _initialized = false;
    private static EntitiesRegistry _i;

    public static EntitiesRegistry i
    {
        get
        {
            if (!_initialized)
            {
                _i = FindObjectOfType<EntitiesRegistry>();
                _i.Initialize();
            }

            return _i;
        }
    }

    private void Initialize()
    {
        if(_initialized) return;
        _initialized = true;
        _i = this;
        // initialize all dictionarites here
    }
    
    private void OnEnable()
    {
        Initialize();
    }
}
