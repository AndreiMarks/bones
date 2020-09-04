using UnityEditor;
using UnityEngine;

/// <summary>
/// MenuItem shortcuts for opening folders/files in the local system.
/// </summary>
public static class FinderUtility
{
    [MenuItem("Tools/Bones/FinderUtility/Open Persistent Data Path")]
    public static void OpenPersistentDataPath()
    {
        Debug.Log("Opening in Finder: " + Application.persistentDataPath);
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
