using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformConstant : MonoBehaviour
{
    public enum _UpdateRate
    {
        Update,FixedUpdate,LateUpdate
    }
    public _UpdateRate UpdateRate;
    [Space]
    public Vector3 Rotation;
    public Space RotationMode;
    [Space]
    public Vector3 Position;
    public Space PositionMode;

    void Update()
    {
        if (UpdateRate != _UpdateRate.Update)
            return;
        MoveOrRotate();
    }
    void FixedUpdate()
    {
        if (UpdateRate != _UpdateRate.FixedUpdate)
            return;
        MoveOrRotate();
    }
    void LateUpdate()
    {
        if (UpdateRate != _UpdateRate.LateUpdate)
            return;
        MoveOrRotate();
    }

    public void MoveOrRotate()
    {
        if (Rotation != Vector3.zero)
        {
            transform.Rotate(Rotation, RotationMode);
        }

        if (Position != Vector3.zero)
        {
            transform.Translate(Rotation, PositionMode);
        }
    }
}
