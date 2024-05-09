using System.IO;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CopyPath : Editor
{
    [MenuItem("Assets/Copy full path", priority = 19)]
    public static void copySelected()
    {
        string path = (Application.dataPath + 
            AssetDatabase.GetAssetOrScenePath(Selection.activeObject));
        path = path.Replace("AssetsAssets", "Assets");
        //path = Path.GetDirectoryName(path);

        EditorGUIUtility.systemCopyBuffer = path;

        Debug.Log($"[Copy Path] Path <b><i>'{path}'</i></b> copied to clipboard.");
    }
}
#endif