using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSelection : MonoBehaviour
{
    public int selected = 0;
    public List<GameObject> gameObjects = new List<GameObject>();
    public GameObject currentSelected { get; set; }
    public bool PerformOnEditor;

    public void HideAll()
    {
        foreach (var item in gameObjects)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SelectItem(GameObject item)
    {
        HideAll();
        currentSelected = item;
        currentSelected.gameObject.SetActive(true);
        selected = gameObjects.IndexOf(item);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR

        if (UnityEditor.Selection.activeGameObject && PerformOnEditor)
        {
            if (gameObjects.Contains(UnityEditor.Selection.activeGameObject))
            {
                SelectItem(UnityEditor.Selection.activeGameObject);
            }
        }
#endif
    }
}
