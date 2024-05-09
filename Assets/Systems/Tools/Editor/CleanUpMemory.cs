#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.Profiling;

public class CleanUpMemory : Editor
{
    static ProfilerRecorder systemMemoryRecorder;

    public static string RecordMemory()
    {
        string r = "";
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        r = $"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB";
        systemMemoryRecorder.Dispose();
        return r;
    }

    [MenuItem("Memory/CleanUp")]
    public static void CleanMemory()
    {
        Debug.Log(RecordMemory());
        AssetDatabase.SaveAssets();
        GC.Collect();
        EditorUtility.UnloadUnusedAssetsImmediate();
        AssetDatabase.Refresh();
    }
}
#endif