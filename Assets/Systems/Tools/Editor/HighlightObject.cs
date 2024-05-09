using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.TerrainTools;


[InitializeOnLoad]
public class HighlightObject : Editor
{
    static HighlightObject()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static Event e;
    static SceneView sv;

    private static void OnSceneGUI(SceneView sceneView)
    {
        sv = sceneView;
        e = Event.current;

        if (e.keyCode == KeyCode.F)
        {
            EditorGUIUtility.PingObject(Selection.activeGameObject);
        }
    }
}
#endif