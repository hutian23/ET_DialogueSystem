#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Timeline.Editor
{
    [EditorTool("Edit Cast Shape", typeof (CastBox))]
    public class CastBoxTool: CastShapeTool<CastBox>
    {
        private readonly BoxBoundsHandle m_boundsHandle = new();

        protected override PrimitiveBoundsHandle boundsHandle
        {
            get
            {
                return m_boundsHandle;
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            foreach (var obj in targets)
            {
                if (!(obj is CastBox castShape) || Mathf.Approximately(castShape.transform.lossyScale.sqrMagnitude, 0f))
                    continue;

                // collider matrix is center multiplied by transform's matrix with custom postmultiplied lossy scale matrix
                var transform = castShape.transform;
                using (new Handles.DrawingScope(Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one)))
                {
                    CopyColliderPropertiesToCollider(castShape);

                    switch (castShape.info.hitboxType)
                    {
                        case HitboxType.Hit:
                            boundsHandle.SetColor(Color.red);
                            break;
                        case HitboxType.Hurt:
                            boundsHandle.SetColor(Color.green);
                            break;
                        case HitboxType.CounterHurt:
                            boundsHandle.SetColor(Color.cyan);
                            break;
                        case HitboxType.Squash:
                            boundsHandle.SetColor(Color.yellow);
                            break;
                        case HitboxType.Throw:
                            boundsHandle.SetColor(Color.magenta);
                            break;
                    }

                    EditorGUI.BeginChangeCheck();
                    
                    //移动handle
                    boundsHandle.center = Handles.PositionHandle(boundsHandle.center, Quaternion.identity);
                    boundsHandle.DrawHandle();

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(castShape, $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");
                        CopyHandlePropertiesToCollider(castShape);
                    }
                }
            }
        }

        //将Script中的数值赋值给handle
        protected override void CopyColliderPropertiesToCollider(CastBox castShape)
        {
            m_boundsHandle.center = TransformColliderCenterToHandleSpace(castShape.transform, castShape.info.center);
            m_boundsHandle.size = Vector3.Scale(castShape.info.size, castShape.transform.lossyScale);
        }

        //将handle中的数值赋值给script
        protected override void CopyHandlePropertiesToCollider(CastBox castShape)
        {
            castShape.info.center = TransformHandleCenterToColliderSpace(castShape.transform, m_boundsHandle.center);
            Vector3 size = Vector3.Scale(m_boundsHandle.size, InvertScaleVector(castShape.transform.lossyScale));
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
            castShape.info.size = size;
        }
    }

    [CustomEditor(typeof (CastBox))]
    [CanEditMultipleObjects]
    public class CastBoxEditor: UnityEditor.Editor
    {
        private SerializedProperty m_center;
        private SerializedProperty m_size;
        // private SerializedProperty m_hitboxType;
        private SerializedProperty m_hitboxName;

        private void OnEnable()
        {
            m_center = serializedObject.FindProperty("info.center");
            m_size = serializedObject.FindProperty("info.size");
            // m_hitboxType = serializedObject.FindProperty("info.hitboxType");
            m_hitboxName = serializedObject.FindProperty("info.boxName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space(4);
            EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Shape"), target);
            EditorGUILayout.Space(4);
            // EditorGUILayout.PropertyField(m_hitboxType);
            EditorGUILayout.LabelField("Hitbox Name", m_hitboxName.stringValue);
            EditorGUILayout.Space(4);
            EditorGUILayout.PropertyField(m_center);
            EditorGUILayout.PropertyField(m_size);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}