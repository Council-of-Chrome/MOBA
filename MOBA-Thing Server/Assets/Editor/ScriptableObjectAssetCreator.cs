using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectAssetCreator
{
    [MenuItem("Assets/Create ScriptableObject")]
    public static void Create()
    {
        MonoScript script = Selection.activeObject as MonoScript;
        Type type = script.GetClass();
        ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type);
        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
        AssetDatabase.CreateAsset(scriptableObject, $"{path}/{Selection.activeObject.name}.asset");
    }

    [MenuItem("Assets/Create ScriptableObject", true)]
    public static bool ValidateCreate()
    {
        MonoScript script = Selection.activeObject as MonoScript;
        return script != null && script.GetClass().IsSubclassOf(typeof(ScriptableObject));
    }
}
