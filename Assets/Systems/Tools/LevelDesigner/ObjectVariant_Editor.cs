using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectVariant_Editor))]
[CanEditMultipleObjects()]
public class ObjectVarianteEditor : Editor
{
    ObjectVariant_Editor t { get { return (ObjectVariant_Editor)target; } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply"))
        {
            if (EditorUtility.DisplayDialog("Apply selection.","Apply Selected Variant? \nWill be delete the other variants","Confirm","Cancel"))
            {
                //PrefabUtility.UnloadPrefabContents(t.gameObject);
                foreach (var item in targets)
                {
                    
                    ObjectVariant_Editor ts = (ObjectVariant_Editor)item;
                    if (!Selection.gameObjects.Contains(ts.gameObject))
                        return;
                    if (!ts)
                        return;
                    if (PrefabUtility.GetNearestPrefabInstanceRoot(ts.gameObject))
                    {
                        PrefabUtility.UnpackPrefabInstance(ts.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    }

                    for (int i = 0; i < ts.variants.Count; i++)
                    {
                        if (ts.variants[i] == null)
                        {
                            ts.variants.RemoveAt(i);
                        }

                        if (i != ts.SelectedVariant)
                        {
                            if (ts.variants[i])
                            {
                                DestroyImmediate(ts.variants[i].gameObject);
                            }

                        }
                        else
                        {
                            ts.variants[i].gameObject.SetActive(true);
                            ts.variants[i].transform.parent = ts.transform.parent;
                            ts.variants[i].transform.SetSiblingIndex(ts.transform.GetSiblingIndex());
                            Selection.activeObject = ts.variants[i];
                            DestroyImmediate(ts.gameObject);
                        }
                    }
                }
            }
        }
    }
}
#endif

[SelectionBase()]
public class ObjectVariant_Editor : MonoBehaviour
{
    public int SelectedVariant = 0;
    int SelectedVariantPreview { get; set; }

    public List<GameObject> variants = new List<GameObject>();
    public bool AutoFindVariations;
    public bool RenameObject;

    public GameObject GetActiveVariant()
    {
        return variants[SelectedVariant];
    }

    public void SelectVariant(int select)
    {
        SelectedVariant = select;
        EnableVariant(SelectedVariant);
    }


    public void EnableVariant(int variant)
    {
        foreach (var item in variants)
        {
            item.gameObject.SetActive(false);
        }

        variants[variant].SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (RenameObject)
        {
            gameObject.name = variants[SelectedVariant].gameObject.name;
        }

#if UNITY_EDITOR
        if (AutoFindVariations)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                variants.Add(transform.GetChild(i).gameObject);
            }
            AutoFindVariations = false;
        }

        Event e = Event.current;

        if (!e.shift)
        {
            if (UnityEditor.Selection.activeGameObject.GetComponentInParent<ObjectVariant_Editor>() == this)
            {
                UnityEditor.Selection.activeGameObject = UnityEditor.Selection.activeGameObject.GetComponentInParent<ObjectVariant_Editor>().gameObject;
            }
        }
        else
        {
            if (!UnityEditor.Selection.activeGameObject)
            {
                if (variants[SelectedVariant].activeSelf)
                    return;
                foreach (var v in variants)
                {
                    v.SetActive(false);
                }
                variants[SelectedVariant].gameObject.SetActive(true);
            }
            else
            {
                if (UnityEditor.Selection.activeGameObject.GetComponentInParent<ObjectVariant_Editor>() == this &&
                    UnityEditor.Selection.activeGameObject != this.gameObject)
                {
                    SelectedVariantPreview = variants.IndexOf(UnityEditor.Selection.activeGameObject);
                    foreach (var v in variants)
                    {
                        v.SetActive(false);
                    }
                    variants[SelectedVariantPreview].gameObject.SetActive(true);
                }
                else
                {
                    foreach (var v in variants)
                    {
                        v.SetActive(false);
                    }
                    variants[SelectedVariant].gameObject.SetActive(true);
                }
            }
        }
        //if (!UnityEditor.Selection.activeGameObject)
        //    return;
        //ObjectVariant_Editor ParentVariant = UnityEditor.Selection.activeGameObject.GetComponentInParent<ObjectVariant_Editor>();

        //if (ParentVariant)
        //{
        //    ParentVariant.EnableVariant(variants.IndexOf(UnityEditor.Selection.activeGameObject));
        //}
#endif
    }
}
