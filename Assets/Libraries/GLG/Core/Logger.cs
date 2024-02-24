using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    [HideInCallstack]
    public static void Log(string message)
    {
        Debug.Log(message);
    }
}
