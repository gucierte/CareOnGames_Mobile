using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum UpdateMode
{
    Awake,OnEnable,Start,Update,FixedUpdate,LateUpdate
}
namespace AnderSystems
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CopyTransform))]
    public class CopyTransformEditor : Editor
    {
        private CopyTransform Target;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            Target =  (CopyTransform)target;

            //Create "Target" Label
            Target.Target = (Transform)EditorGUILayout.ObjectField("Target", Target.Target, typeof(Transform), true);

            //Create "Unparent" Label
            Target.RemoveParent = EditorGUILayout.ToggleLeft("Remove Parent", Target.RemoveParent, GUILayout.Width(150));

            //Create UpdateMode Enum
            Target.UpdateMode = (UpdateMode)EditorGUILayout.EnumPopup("Update Mode", Target.UpdateMode);

            //Copy Position
            EditorGUILayout.HelpBox("Copy Position", MessageType.None);
            Target.CopyPos = EditorGUILayout.BeginToggleGroup("Copy Position", Target.CopyPos);
            CreateCopyLabel(Target.CopyPosition);
            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Space(10);

            //Copy Rotation
            EditorGUILayout.HelpBox("Copy Rotation", MessageType.None);
            Target.CopyRot = EditorGUILayout.BeginToggleGroup("Copy Rotation", Target.CopyRot);
            CreateCopyLabel(Target.CopyRotation);
            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Space(10);

            //Set Parent
            EditorGUILayout.HelpBox("Set Parent", MessageType.None);
            Target.ReParent = EditorGUILayout.BeginToggleGroup("Set Parent", Target.ReParent);
            CreateCopyLabel(Target.SetParent);
            EditorGUILayout.EndToggleGroup();
        }

        public void CreateCopyLabel(CopyTransform.CopyTr _target)
        {
            

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Copy Axis:", GUILayout.Width(100));
            //_target.X = EditorGUILayout.ToggleLeft("X", _target.X, GUILayout.Width(30));
            //_target.Y = EditorGUILayout.ToggleLeft("Y", _target.Y, GUILayout.Width(30));
            //_target.Z = EditorGUILayout.ToggleLeft("Z", _target.Z, GUILayout.Width(30));
            //EditorGUILayout.EndHorizontal();
            _target.Lerping = EditorGUILayout.FloatField("Lerping", _target.Lerping);

            EditorGUILayout.BeginHorizontal();
            _target.UseCustomTarget = EditorGUILayout.ToggleLeft("Use Custom Target", _target.UseCustomTarget, GUILayout.Width(150));

            if (_target.UseCustomTarget)
            {
                _target.Target = (Transform)EditorGUILayout.ObjectField("| Target", _target.Target, typeof(Transform), true);
            }
            else
            {
                //_target.Target = Target.Target;
            }
            EditorGUILayout.EndHorizontal();
            /*
            _target.UseCustomTarget = EditorGUILayout.Toggle("Use Custom Target", _target.UseCustomTarget, GUILayout.Width(30));

            if (!_target.UseCustomTarget)
            {
                _target.Target = Target.Target;
            } else
            {
                _target.Target = (Transform)EditorGUILayout.ObjectField("Target", _target.Target, typeof(Transform), true);
            }*/
        }
    }
#endif

    public class CopyTransform : MonoBehaviour
    {
        [SerializeField]
        public UpdateMode UpdateMode;
        public bool RemoveParent;
        public Transform Target;
        public bool CopyPos, CopyRot, ReParent;
        [System.Serializable]
        public class CopyTr
        {
            public bool UseCustomTarget;
            public Transform Target;
            public bool X, Y, Z;
            public float Lerping;
        }
        [SerializeField]
        public CopyTr CopyPosition;

        [SerializeField]
        public CopyTr CopyRotation;

        [SerializeField]
        public CopyTr SetParent;

        public void ChangeTarget(Transform newTarget)
        {
            Target = newTarget;
        }

        public enum CopyType
        {
            Position, Rotation
        }
        /// <summary>
        /// Copy Transform
        /// </summary>
        /// <param name="Target">Target to apply transform</param>
        /// <param name="tr">Target Params (to copy)</param>
        /// <param name="toCopy">Copy Type (Position or Rotation)</param>
        public static void ToCopy(Transform Target, CopyTr tr, CopyType toCopy)
        {
            //Copy Pos
            switch (toCopy)
            {
                case CopyType.Position:
                    if (tr.Lerping == 0)
                    {
                        Target.transform.position = tr.Target.position;
                    }
                    else
                    {
                        Target.transform.position = Vector3.Lerp(Target.transform.position, tr.Target.position, tr.Lerping * Time.deltaTime);
                    }
                    break;

                case CopyType.Rotation:
                    if (tr.Lerping == 0)
                    {
                        Target.transform.rotation = tr.Target.rotation;
                    }
                    else
                    {
                        Target.transform.rotation = Quaternion.Lerp(Target.transform.rotation, tr.Target.rotation, tr.Lerping * Time.deltaTime);
                    }
                    break;
            }
        }

        public void CopyPosAndRot()
        {
            if (ReParent)
            {
                if (transform.parent != Target)
                {
                    transform.parent = Target;
                }
            }

            if (!CopyPosition.UseCustomTarget)
            {
                CopyPosition.Target = Target;
            }

            if (!CopyRotation.UseCustomTarget)
            {
                CopyRotation.Target = Target;
            }

            if (CopyPos)
            {
                ToCopy(this.transform, CopyPosition, CopyType.Position);
            }

            if (CopyRot)
            {
                ToCopy(this.transform, CopyRotation, CopyType.Rotation);
            }
        }

        void Start()
        {
            if (RemoveParent)
            {
                transform.parent = null;
            }
        }

        //Call Voids
        void Update()
        {
            if (UpdateMode != UpdateMode.Update)
                return;
            CopyPosAndRot();
        }
        void FixedUpdate()
        {
            if (UpdateMode != UpdateMode.FixedUpdate)
                return;
            CopyPosAndRot();
        }
        void LateUpdate()
        {
            if (UpdateMode != UpdateMode.LateUpdate)
                return;
            CopyPosAndRot();
        }
    }
}
