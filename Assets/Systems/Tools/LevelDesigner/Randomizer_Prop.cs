using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Randomizer_Prop : MonoBehaviour
{
    public int CurrentVariant;
    public List<GameObject> Variants = new List<GameObject>();
    public bool includeEmpty = false;
    public bool Get = true;


    private void OnValidate()
    {
        if (Variants.Count <= 0)
        {
            foreach (var item in GetComponentsInChildren<Transform>())
            {
                if (item.gameObject != this.gameObject)
                {
                    Variants.Add(item.gameObject);
                }
            } 
        }
    }

    public void SetVariant(int Variant)
    {
        DisableAll();

        if (Variant >= 0)
        {
            Variants[Variant].SetActive(true);
        }
    }

    public void DisableAll()
    {
        foreach (var item in Variants)
        {
            item.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (Variants.Count <= 0)
            return;
        if (Get)
        {
            if (includeEmpty)
            {
                CurrentVariant = Random.Range(-1, Variants.Count);
            } else
            {
                CurrentVariant = Random.Range(0, Variants.Count);
            }
            SetVariant(CurrentVariant);

            Get = false;
        }
    }
}
