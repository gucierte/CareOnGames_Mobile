using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransforms : MonoBehaviour
{
    public Transform Target;
    public enum _Method
    {
        LateUpdate,FixedUpdate
    }
    [Space]
    public _Method Method;
    [Range(0,10)]
    public float PosLerp,RotLerp,ScaleLerp;
    public bool RemoveParent;
    public bool YOnly;

    void OnEnable()
    {
        if (RemoveParent)
        {
            transform.parent = null;
        }
    }

    void FixedUpdate()
    {
        if (Method != _Method.FixedUpdate)
            return;
        CopyLocation(Target.position);
        CopyRotation(Target.rotation);
        CopyScale(Target.localScale);
    }

    void LateUpdate()
    {
        if (Method != _Method.LateUpdate)
            return;
        CopyLocation(Target.position);
        if (!YOnly)
        {
            CopyRotation(Target.rotation);
        } else
        {
            CopyRotation(Quaternion.Euler(transform.eulerAngles.x, Target.eulerAngles.y, transform.eulerAngles.z));
        }
        CopyScale(Target.localScale);

    }

    public void CopyLocation(Vector3 Pos)
    {
        if (PosLerp <= 0)
            return;
        if (PosLerp >= 10)
        {
            transform.position = Pos;
        } else
        {
            if (PosLerp > 0)
            {
                transform.position = Vector3.Lerp(transform.position, Target.transform.position, PosLerp * Time.deltaTime);
            }
        }
    }
    public void CopyRotation(Quaternion Rot)
    {
        if (RotLerp <= 0)
            return;
        if (RotLerp >= 10)
        {
            transform.rotation = Rot;
        }
        else
        {
            if (RotLerp > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Target.transform.rotation, RotLerp * Time.deltaTime);
            }
        }
    }
    public void CopyScale(Vector3 Scale)
    {
        if (ScaleLerp <= 0)
            return;
        if (ScaleLerp >= 10)
        {
            transform.localScale = Scale;
        }
        else
        {
            if (ScaleLerp > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Target.transform.localScale, ScaleLerp * Time.deltaTime);
            }
        }
    }



}
