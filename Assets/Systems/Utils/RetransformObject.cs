using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetransformObject : MonoBehaviour
{
    public Transform From;
    public Transform To;
    public float Lerp;
    [System.Serializable]
    public enum reparent
    {
        Reparent, DontReparent, ReparentOnly
    }
    [SerializeField]
    public reparent Reparent;
    public bool DisableWhenDone;
    int Index;

    public void OnEnable()
    {
        Index = To.GetSiblingIndex() + To.parent.childCount;
        FixedUpdate();
        if (DisableWhenDone)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        RectTransform rFrom = (From as RectTransform);
        RectTransform rTo = (To as RectTransform);
        if (rFrom != null && rTo != null)
        {
            switch (Reparent)
            {
                case reparent.Reparent:
                    rFrom.transform.parent = rTo.transform.parent;
                    rFrom.transform.SetSiblingIndex(Index);

                    rFrom.position = Vector3.Lerp(rFrom.position, rTo.position, Lerp * Time.deltaTime);
                    rFrom.rotation = Quaternion.Lerp(rFrom.rotation, rTo.rotation, Lerp * Time.deltaTime);
                    rFrom.anchoredPosition = Vector3.Lerp(rFrom.anchoredPosition, rTo.anchoredPosition, Lerp * Time.deltaTime);
                    rFrom.anchorMax = Vector2.Lerp(rFrom.anchorMax, rTo.anchorMax, Lerp * Time.deltaTime);
                    rFrom.anchorMin = Vector2.Lerp(rFrom.anchorMin, rTo.anchorMin, Lerp * Time.deltaTime);
                    rFrom.localScale = Vector3.Lerp(rFrom.localScale, rTo.localScale, Lerp * Time.deltaTime);
                    rFrom.sizeDelta = Vector2.Lerp(rFrom.sizeDelta, rTo.sizeDelta, Lerp * Time.deltaTime);
                    break;
                case reparent.DontReparent:

                    rFrom.position = Vector3.Lerp(rFrom.position, rTo.position, Lerp * Time.deltaTime);
                    rFrom.rotation = Quaternion.Lerp(rFrom.rotation, rTo.rotation, Lerp * Time.deltaTime);
                    rFrom.anchoredPosition = Vector3.Lerp(rFrom.anchoredPosition, rTo.anchoredPosition, Lerp * Time.deltaTime);
                    rFrom.anchorMax = Vector2.Lerp(rFrom.anchorMax, rTo.anchorMax, Lerp * Time.deltaTime);
                    rFrom.anchorMin = Vector2.Lerp(rFrom.anchorMin, rTo.anchorMin, Lerp * Time.deltaTime);
                    rFrom.localScale = Vector3.Lerp(rFrom.localScale, rTo.localScale, Lerp * Time.deltaTime);
                    rFrom.sizeDelta = Vector2.Lerp(rFrom.sizeDelta, rTo.sizeDelta, Lerp * Time.deltaTime);
                    break;
                case reparent.ReparentOnly:
                    rFrom.transform.parent = rTo.transform.parent;
                    rFrom.transform.SetSiblingIndex(Index);
                    break;
                default:
                    break;
            }
        } else
        {
            switch (Reparent)
            {
                case reparent.Reparent:
                    From.transform.parent = To.transform.parent;
                    From.transform.SetSiblingIndex(Index);
                    From.transform.position = Vector3.Lerp(From.position, To.position, Lerp * Time.deltaTime);
                    From.transform.rotation = Quaternion.Lerp(From.rotation, To.rotation, Lerp * Time.deltaTime);
                    From.transform.localScale = Vector3.Lerp(From.localScale, To.localScale, Lerp * Time.deltaTime);
                    break;
                case reparent.DontReparent:
                    From.transform.position = Vector3.Lerp(From.position, To.position, Lerp * Time.deltaTime);
                    From.transform.rotation = Quaternion.Lerp(From.rotation, To.rotation, Lerp * Time.deltaTime);
                    From.transform.localScale = Vector3.Lerp(From.localScale, To.localScale, Lerp * Time.deltaTime);
                    break;
                case reparent.ReparentOnly:
                    From.transform.parent = To.transform.parent;
                    From.transform.SetSiblingIndex(Index);
                    break;
                default:
                    break;
            }

        }
    }

    private void OnValidate()
    {
        if (To == null)
        {
            To = this.transform;
        }
    }
}
