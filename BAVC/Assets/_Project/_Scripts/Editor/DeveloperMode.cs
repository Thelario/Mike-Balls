#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class DeveloperMode 
{
    [MenuItem("Developer/Clear Saves")]
    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        
        Debug.Log("<color=Color.blue>Developer: </color>" + "Clearing all saves from player prefs in Developer Mode.");
    }
}

#endif