using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColor : MonoBehaviour
{
    public Color GlobalColor = Color.white;
    [System.Serializable]
    public class target
    {
        [HideInInspector] public string name;
        public Color color;
        public string colorProperty = "_BaseColor";

        public Renderer renderer;
        public int materialIndex;
        public void ChangeRendererColor(Color newColor)
        {
            renderer.materials[materialIndex].SetColor(colorProperty, newColor);
        }
    }
    [SerializeField]
    public List<target> targets = new List<target>();

    public void ChangeColors()
    {
        foreach (var item in targets)
        {
            item.ChangeRendererColor(item.color * GlobalColor);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (targets.Count < 1)
        {
            targets.Add(new target() { renderer = GetComponent<Renderer>(), color = Color.white, colorProperty = "_BaseColor", materialIndex = 0 });
        }

        ChangeColors();
    }

    private void OnValidate()
    {
        foreach (var item in targets)
        {
            if (item.renderer.gameObject)
            {
                item.name = item.renderer.gameObject.name;
            }
        }
    }
}
