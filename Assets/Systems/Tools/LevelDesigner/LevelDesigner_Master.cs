#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor.UIElements;
using UnityEditor.SearchService;

[InitializeOnLoad]
public class LevelDesigner_Master : Editor
{
    static SceneView sv;
    static LevelDesigner_Master()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    static Event e;

    [MenuItem("Level Designer/SelectNextObj #TAB")]
    public static void SelectNextObj()
    {
        Transform parent = Selection.activeTransform.transform.parent;
        int Index = (int)Mathf.Repeat(Selection.activeTransform.transform.GetSiblingIndex() + 1, parent.childCount);
        Selection.activeTransform = parent.GetChild(Index);
    }

    public static void ApplyRotation(GameObject target)
    {
        List<Transform> childs = target.transform.GetComponentsInChildren<Transform>().ToList();

        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i].transform.parent != target) { childs.RemoveAt(i); }
        }

        foreach (var item in childs)
        {
            item.transform.parent = null;
            target.transform.eulerAngles = Vector3.zero;
            item.transform.parent = target.transform;
        }
    }
    public static void ApplyScale(GameObject target)
    {
        List<Transform> childs = target.transform.GetComponentsInChildren<Transform>().ToList();

        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i].transform.parent != target) { childs.RemoveAt(i); }
        }

        foreach (var item in childs)
        {
            item.transform.parent = null;
            target.transform.localScale = Vector3.one;
            item.transform.parent = target.transform;
        }
    }

    [MenuItem("GameObject/Apply Transforms/Apply Rotation")]
    public static void ApplyRotationToSelected()
    {
        foreach (var item in Selection.gameObjects)
        {
            ApplyRotation(item);
        }
    }

    [MenuItem("GameObject/Apply Transforms/Apply Scale")]
    public static void ApplyScaleToSelected()
    {
        foreach (var item in Selection.gameObjects)
        {
            ApplyScale(item);
        }
    }

    public static void UnGroupChild(Transform target)
    {
        int ChildCount = target.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            target.GetChild(0).parent = target.parent;
        }
    }

    public static void UnGroupSelectionChild()
    {
        UnGroupChild(Selection.activeTransform);
    }


    static void OnSceneGUI(SceneView sceneView)
    {
        //Contex menu
        sv = sceneView;
        e = Event.current;
        string[] LowerBottomLines = new string[10];
        string LowerBottomText = "";
        int verticesCount = 0;


        //foreach (var item in Selection.gameObjects)
        //{
        //    if (item.scene.IsValid())
        //    {
        //        if (item.GetComponent<MeshFilter>())
        //        {
        //            verticesCount += item.GetComponent<MeshFilter>().mesh.vertexCount;
        //        }

        //        foreach (var child in item.GetComponentsInChildren<MeshFilter>())
        //        {
        //            verticesCount += child.mesh.vertexCount;
        //        }
        //    }
        //}


        if (Selection.count > 1)
        {
            LowerBottomLines[2] = (("<size=32><b>(" + Selection.count + ") Selected</b></size>"));
        }
        else
        {
            if (Selection.count > 0)
            {
                LowerBottomLines[2] = ("<size=32><b>" + Selection.activeObject.name + "</b></size>");
            }
            else
            {
                LowerBottomLines[2] = "";
            }
        }

        if (verticesCount > 0)
        {
            LowerBottomLines[2] += $" (Vertices: {verticesCount})";
        }

        //if (Selection.activeTransform != null)
        //{
        //    if (e.type == EventType.MouseDrag && e.shift)
        //    {
        //        Selection.activeTransform.gameObject.SetActive(false);
        //        RaycastHit hit;
        //        Vector3 pos = GetObjectMousePos(out hit, e.mousePosition, POG_Distance);
        //        if (hit.collider)
        //        {
        //            Selection.activeTransform.position = hit.collider.transform.position;
        //            Selection.activeTransform.rotation = hit.collider.transform.rotation;
        //        }
        //        Selection.activeTransform.gameObject.SetActive(true);
        //    }
        //}


        //if (e.button == 1 && e.control && !e.shift)
        //{
            //if (e.type == EventType.MouseDown)
            //{
                //if (Selection.count > 1) { SceneContexMenuEditor.menu.AddItem(new GUIContent("Group"), false, Group, 1); } else { SceneContexMenuEditor.menu.AddDisabledItem(new GUIContent("Group"), false); }
                //if (Selection.count > 1) { SceneContexMenuEditor.menu.AddItem(new GUIContent("Ungroup Child"), false, UnGroupSelectionChild); } else { SceneContexMenuEditor.menu.AddDisabledItem(new GUIContent("Ungroup Child"), false); }

                //menu.ShowAsContext();
            //}
        //}

        if (e.keyCode == KeyCode.M && e.shift && e.type == EventType.KeyDown)
        {
            mirrorAxis = (int)Mathf.Repeat(mirrorAxis + 1, 3);

            if (mirrorAxis == 0)
            {
                Selection.activeTransform.localScale = new Vector3(Selection.activeTransform.localScale.x, Selection.activeTransform.localScale.y, Selection.activeTransform.localScale.z * -1);
            }

            if (mirrorAxis == 1)
            {
                Selection.activeTransform.localScale = new Vector3(Selection.activeTransform.localScale.x * -1, Selection.activeTransform.localScale.y, Selection.activeTransform.localScale.z);
            }

            if (mirrorAxis == 2)
            {
                Selection.activeTransform.localScale = new Vector3(Selection.activeTransform.localScale.x * -1, Selection.activeTransform.localScale.y * -1, Selection.activeTransform.localScale.z);
            }

            if (mirrorAxis == 3)
            {
                Selection.activeTransform.localScale = new Vector3(Selection.activeTransform.localScale.x, Selection.activeTransform.localScale.y * -1, Selection.activeTransform.localScale.z * -1);
            }
        }
        //ShortKeys
        //if (e.isKey)
        //{
        if (Selection.gameObjects.Length > 0)
        {
            if (e.isScrollWheel && e.control)
            {
                POG_Rotation += ((int)(e.delta.y / 2)) * 15;

                for (int i = 0; i < Selection.count; i++)
                {
                    //POG_Rotation = ((int)(Vector3.Distance(sv.camera.WorldToScreenPoint(Selection.activeTransform.position), sv.camera.ScreenToWorldPoint(e.mousePosition)) / 45)) * 15;
                    //Selection.gameObjects[0].transform.eulerAngles = new Vector3(Selection.gameObjects[i].transform.eulerAngles.x, POG_Rotation, Selection.gameObjects[i].transform.eulerAngles.z);
                    Selection.gameObjects[i].transform.eulerAngles = new Vector3(Selection.gameObjects[i].transform.eulerAngles.x, POG_Rotation, Selection.gameObjects[i].transform.eulerAngles.z);
                    e.Use();
                }
            }

            if (e.isScrollWheel && e.shift)
            {
                for (int i = 0; i < Selection.count; i++)
                {
                    if (Selection.gameObjects[i].GetComponent<ObjectVariant_Editor>())
                    {
                        ObjectVariant_Editor Variant = Selection.gameObjects[i].GetComponent<ObjectVariant_Editor>();

                        Variant.SelectVariant((int)Mathf.Repeat(Variant.SelectedVariant + ((int)(e.delta.y / 2)), Variant.variants.Count));
                        e.Use();
                    }
                }
            }

            if (Selection.count <= 1)
            {
                if (Selection.activeGameObject.GetComponent<ObjectVariant_Editor>())
                {
                    LowerBottomLines[1] = "(" + Selection.activeGameObject.GetComponent<ObjectVariant_Editor>().variants.Count + ") variants on this object.";
                    LowerBottomLines[2] += "<size=28> / " + Selection.activeGameObject.GetComponent<ObjectVariant_Editor>().GetActiveVariant().name + "</size>";
                }
            }
        }

        //}


        //if (e.isScrollWheel)
        //{
        //    POG_Rotation += e.delta.y * 2;
        //    e.Use();
        //}

        if (e.isKey && e.keyCode != KeyCode.P && e.keyCode != KeyCode.F)
        {
            if (e.control && e.shift)
            {
                PutObjectOnGround(Selection.activeTransform, e.mousePosition);
            }
            else
            {
                POG_Axis = 0;
            }
        }


        if (e.isKey)
        {

            if (e.keyCode == KeyCode.G && e.control)
            {
                Group();
            }

            if (e.keyCode == KeyCode.Y && e.control)
            {
                //if (POG_Axis == 1)
                //{
                    //POG_Axis = -1;
                //} else
                {
                    POG_Axis = 1;
                }
            }

            if (e.keyCode == KeyCode.X && e.control)
            {
                //if (POG_Axis == 2)
                //{
                    //POG_Axis = -2;
                //}
                //else
                {
                    POG_Axis = 2;
                }
            }

            if (e.keyCode == KeyCode.Z && e.control)
            {
                //if (POG_Axis == 3)
                //{
                //    POG_Axis = -3;
                //}
                //else
                {
                    POG_Axis = 3;
                }
            }
        }

        bool disableSceneGUI = false;

        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (Selection.gameObjects[i].GetComponent<ParticleSystem>())
            {
                disableSceneGUI = true;
            }
        }

        if (!disableSceneGUI)
        {
            Handles.BeginGUI();
            GUIStyle TitleStyle = new GUIStyle();
            TitleStyle.padding = new RectOffset(10, 0, sv.camera.scaledPixelHeight - 112, 0);
            TitleStyle.normal.textColor = Color.white;

            GUIStyle ShadowStyle = new GUIStyle();
            ShadowStyle.padding = new RectOffset(15, 0, sv.camera.scaledPixelHeight - 112, 0);
            ShadowStyle.normal.textColor = Color.black;

            foreach (var item in LowerBottomLines)
            {
                LowerBottomText += item + "\n";
            }

            if (POG_Axis != 0)
            {
                LowerBottomLines[3] = "Aling: " + POG_Axis;
            }

            GUILayout.Label(LowerBottomText, TitleStyle);
            GUILayout.Label(LowerBottomText, ShadowStyle);

            Handles.EndGUI();
        }
    }

    static int mirrorAxis;

    //Utilities

    /// <summary>
    /// Create new object on Default Coords, Parent and Numerable Name
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public static GameObject CreateNewObject(string Name)
    {
        Transform parent = default;
        if (Selection.activeTransform)
        {
            parent = Selection.activeTransform.parent;
        }


        string _name = Name + "_" + GetNumOfObjectsStartWith(Name + "_", parent).Count;
        GameObject Obj = new GameObject(_name);

        Obj.transform.position = GetObjectDefaultPos();

        if (Selection.activeTransform)
        {
            Obj.transform.parent = Selection.activeTransform;
        }

        Selection.activeGameObject = Obj;

        return Obj;
    }
    public static Vector3 GetObjectDefaultPos(float MaxDistance = 10)
    {
        RaycastHit hit;
        Vector3 Result;

        if (Physics.Raycast(sv.camera.transform.position, sv.camera.transform.forward * MaxDistance, out hit))
        {
            Result = hit.point;
        }
        else
        {
            Result = (sv.camera.transform.forward * MaxDistance) + sv.camera.transform.position;
        }

        return Result;
    }
    public static Vector3 GetObjectDefaultPos(out RaycastHit RayHit, float MaxDistance = 10)
    {
        RaycastHit hit;
        Vector3 Result;

        if (Physics.Raycast(sv.camera.transform.position, sv.camera.transform.forward * MaxDistance, out hit))
        {
            Result = hit.point;
        }
        else
        {
            Result = (sv.camera.transform.forward * MaxDistance) + sv.camera.transform.position;
        }
        RayHit = hit;
        return Result;
    }

    public static Vector3 GetObjectMousePos(out RaycastHit RayHit, Vector2 MousPos, float MaxDistance = 10)
    {
        RaycastHit hit;
        Vector3 Result;

        Vector3 mpos = new Vector3(MousPos.x, -MousPos.y + sv.camera.pixelHeight);

        if (Physics.Raycast(sv.camera.ScreenPointToRay(mpos), out hit))
        {
            Result = hit.point;
        }
        else
        {
            Result = (sv.camera.ScreenToViewportPoint(mpos)) + sv.camera.transform.position;
        }
        RayHit = hit;
        return Result;
    }
    /// <summary>
    /// Get Center of Transforms
    /// </summary>
    /// <param name="transforms"></param>
    /// <returns></returns>
    public static Vector3 FindCenterOfTransforms(Transform[] transforms)
    {
        Bounds bounds = new Bounds(transforms[0].position, Vector3.zero);

        for (int i = 0; i < transforms.Length; i++)
        {
            bounds.Encapsulate(transforms[i].position);
        }

        return bounds.center;
    }
    public static List<GameObject> GetNumOfObjectsStartWith(string StartWith, Transform Parent)
    {
        List<GameObject> ObjsAlreadyExist = FindObjectsOfType<GameObject>().ToList();
        if (ObjsAlreadyExist.Count >= 1)
        {
            for (int i = 0; i < ObjsAlreadyExist.Count - 1; i++)
            {
                if (ObjsAlreadyExist[i].transform.parent != Parent)
                {
                    ObjsAlreadyExist.RemoveAt(i);
                }

                if (!ObjsAlreadyExist[i].name.StartsWith(StartWith))
                {
                    ObjsAlreadyExist.RemoveAt(i);
                }
            }
        }

        return ObjsAlreadyExist;
    }



    static int POG_Distance = 10;
    static int POG_Axis = 0;
    static float POG_Rotation = 0;
    //Put on Ground (POG)
    public static void PutObjectOnGround(Transform obj, Vector2 MousPos)
    {
        obj.gameObject.SetActive(false);
        RaycastHit hit;
        Vector3 pos = GetObjectMousePos(out hit, MousPos, POG_Distance);

        if (hit.collider)
        {
            obj.transform.position = pos;
        }

        obj.gameObject.SetActive(true);
        Vector3 eulerRotationMultipiler = new Vector3();

        //Y
        if (POG_Axis == 1)
        {
            obj.transform.up = hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.up = hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(0, POG_Rotation, 0);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }
        if (POG_Axis == -1)
        {
            obj.transform.up = -hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.up = -hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(0, POG_Rotation, 0);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }

        //X
        if (POG_Axis == 2)
        {
            obj.transform.right = hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.right = hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(POG_Rotation, 0, 0);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }
        if (POG_Axis == -2)
        {
            obj.transform.right = -hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.right = -hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(POG_Rotation, 0, 0);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }

        //Z
        if (POG_Axis == 3)
        {
            obj.transform.forward = hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.forward = hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(0, 0, POG_Rotation);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }
        if (POG_Axis == -3)
        {
            obj.transform.forward = hit.normal;

            Transform _tmp = new GameObject("temp").transform;
            _tmp.transform.position = obj.transform.position;
            _tmp.transform.parent = obj.parent;
            _tmp.transform.forward = hit.normal;
            obj.parent = _tmp;

            obj.localEulerAngles = new Vector3(0, 0, POG_Rotation);

            eulerRotationMultipiler = new Vector3(0, 1, 0);

            obj.parent = _tmp.parent;

            DestroyImmediate(_tmp.gameObject);
        }


        if (POG_Axis == 0)
        {
            obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, POG_Rotation, obj.transform.eulerAngles.z);
        }
        //obj.transform.eulerAngles = new Vector3(eulerRotationMultipiler.x + POG_Rotation, eulerRotationMultipiler.y + POG_Rotation, eulerRotationMultipiler.z + POG_Rotation);
        //obj.transform.localEulerAngles += new Vector3(eulerRotationMultipiler.z * POG_Rotation, eulerRotationMultipiler.y * POG_Rotation, eulerRotationMultipiler.x * POG_Rotation);
        //obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, POG_Rotation, obj.transform.localEulerAngles.z);
        //obj.transform.localEulerAngles = obj.TransformDirection(new Vector3(obj.transform.localEulerAngles.x, POG_Rotation, obj.transform.localEulerAngles.z));
        //obj.transform.localEulerAngles += obj.TransformDirection(new Vector3(eulerRotationMultipiler.z * POG_Rotation, eulerRotationMultipiler.y * POG_Rotation, eulerRotationMultipiler.x * POG_Rotation));
        //obj.transform.localRotation = Quaternion.Euler(obj.transform.localRotation.eulerAngles.x, obj.transform.localRotation.eulerAngles.y + POG_Rotation, obj.transform.localRotation.eulerAngles.z);
    }


    //Create Group
    [MenuItem("GameObject/Group")]
    static void Group()
    {
        if (Selection.count > 1)
        {
            Group(Selection.transforms);
        } //else
        //{
            //Debug.LogWarning("Please select two or more objects to group.");
            //CreateNewObject("Group");
        //}
    }

    [MenuItem("GameObject/Ungroup Child")]
    static void UnGroupChild()
    {
        UnGroupSelectionChild();
    }

    static void Group(object obj)
    {
        Group();
    }
    public static Transform Group(Transform[] Objects)
    {
        //Create the group object
        string _name = "Group_" + GetNumOfObjectsStartWith("Group_", Objects[0].parent).Count;
        GameObject Group = new GameObject(_name);

        //Grouping the object
        Group.transform.position = FindCenterOfTransforms(Objects);
        Group.transform.parent = Objects[0].transform.parent;

        for (int i = 0; i < Objects.Length; i++)
        {
            Objects[i].transform.parent = Group.transform;
        }

        Selection.activeObject = Group;

        return Group.transform;
    }
}
#endif
