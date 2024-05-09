using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[InitializeOnLoad]
public class RaycastSelection : Editor
{
    static RaycastSelection()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static Event e;
    static SceneView sv;
    static bool use;

    private static async void OnSceneGUI(SceneView sceneView)
    {
        sv = sceneView;
        e = Event.current;

        use = (e.alt);

        if (!use || e.type == EventType.MouseDrag)
            return;
        Vector3 m = e.mousePosition;
        m.y = sv.camera.scaledPixelHeight - e.mousePosition.y;

        Ray r = sv.camera.ScreenPointToRay(m);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit, 9999))
        {
            Handles.DrawOutline(new GameObject[] { hit.collider.gameObject }, Color.white, 0f);

            if (e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
            {
                Selection.activeGameObject = hit.collider.gameObject;
                Debug.Log("Left Click");
            }
        }
        Graphic frontGraphic = null;

        foreach (var graphic in FindObjectsByType<Graphic>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (!graphic.raycastTarget || graphic.canvasRenderer.cull || graphic.depth == -1)
                continue;

            if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, m, sv.camera, graphic.raycastPadding))
                continue;

            if (sv.camera != null && sv.camera.WorldToScreenPoint(graphic.rectTransform.position).z > sv.camera.farClipPlane)
                continue;

            if (graphic.Raycast(m, sv.camera))
            {
                if (frontGraphic == null || frontGraphic.depth < graphic.depth)
                {
                    frontGraphic = graphic;
                }
            }
        }


        List<GameObject> selectedObjects = new List<GameObject>();
        if (e.shift) { selectedObjects = Selection.gameObjects.ToList(); }

        if (frontGraphic)
        {
            //Gizmos.DrawCube(frontGraphic.rectTransform.position, Vector3.one);
            Handles.color = new Color(1,1,1,0.2f);
            Handles.DrawWireCube(frontGraphic.rectTransform.position, frontGraphic.rectTransform.rect.size);

            if (e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
            {

                if (selectedObjects.Contains(frontGraphic.gameObject))
                {
                    selectedObjects.Remove(frontGraphic.gameObject);
                }
                else
                {
                    selectedObjects.Add(frontGraphic.gameObject);
                }
                await Task.Delay(100);
                Selection.objects = selectedObjects.ToArray();
            }
        }
    }
}
#endif