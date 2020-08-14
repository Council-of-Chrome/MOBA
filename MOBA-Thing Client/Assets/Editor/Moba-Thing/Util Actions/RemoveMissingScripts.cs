using System.Linq;
using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : Editor
{
    [MenuItem("Moba-Thing/Util Editor Actions/Remove Missing References in GameObjects")]
    public static void Remove()
    {
        var objs = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = objs.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        Debug.Log($"Removed {count} missing scripts from gameobjects in current scene.");
    }
}