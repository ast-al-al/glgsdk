using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ProjectSettingsHelper : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        if (string.IsNullOrEmpty(PlayerSettings.keystorePass) || string.IsNullOrEmpty(PlayerSettings.keyaliasPass))
        {
            string path = Application.dataPath;
            path = path.Replace("/Assets", "/Key/Key.txt");
            if (!File.Exists(path)) return;
            using (StreamReader sr = new StreamReader(path))
            {
                string pass = sr.ReadLine();
                PlayerSettings.keystorePass = pass;
                PlayerSettings.keyaliasPass = pass;
            }
        }
    }
#endif
}