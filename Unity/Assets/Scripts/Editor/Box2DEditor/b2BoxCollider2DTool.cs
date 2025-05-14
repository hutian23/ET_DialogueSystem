#if UNITY_EDITOR
using Timeline;
using Timeline.Editor;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ET
{
    [EditorTool("Edit SceneBox Shape", typeof(b2BoxCollider2D))]
    public class b2BoxCollider2DTool : CastShapeTool<b2BoxCollider2D>
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
            foreach (UnityEngine.Object obj in targets)
            {
                if (!(obj is b2BoxCollider2D castShape) || Mathf.Approximately(castShape.transform.lossyScale.sqrMagnitude, 0f))
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
                        case HitboxType.Squash:
                            boundsHandle.SetColor(Color.yellow);
                            break;
                        case HitboxType.Throw:
                            boundsHandle.SetColor(Color.blue);
                            break;
                        case HitboxType.Proximity:
                            boundsHandle.SetColor(Color.magenta);
                            break;
                        case HitboxType.Other:
                            Gizmos.color = Color.gray;
                            break;
                        case HitboxType.Gizmos:
                            boundsHandle.SetColor(Color.cyan);
                            break;  
                        case HitboxType.None:
                            boundsHandle.SetColor(Color.white);
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
        
        protected override void CopyColliderPropertiesToCollider(b2BoxCollider2D castShape)
        {
            m_boundsHandle.center = TransformColliderCenterToHandleSpace(castShape.transform, castShape.info.center);
            m_boundsHandle.size = Vector3.Scale(castShape.info.size, castShape.transform.lossyScale);
        }

        protected override void CopyHandlePropertiesToCollider(b2BoxCollider2D castShape)
        {
            castShape.info.center = TransformHandleCenterToColliderSpace(castShape.transform, m_boundsHandle.center);
            Vector3 size = Vector3.Scale(m_boundsHandle.size, InvertScaleVector(castShape.transform.lossyScale));
            size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
            castShape.info.size = size;
        }
    }

    [CustomEditor(typeof(b2BoxCollider2D))]
    [CanEditMultipleObjects]
    public class b2BoxCollider2DEditor: Editor
    {
        private SerializedProperty m_center;
        private SerializedProperty m_size;
        private SerializedProperty m_hitboxName;
        private SerializedProperty m_hitboxType;
        private SerializedProperty m_IsTrigger;
        private SerializedProperty m_TagType;

        private void OnEnable()
        {
            m_center = serializedObject.FindProperty("info.center");
            m_size = serializedObject.FindProperty("info.size");
            m_hitboxName = serializedObject.FindProperty("info.boxName");
            m_hitboxType = serializedObject.FindProperty("info.hitboxType");
            m_TagType = serializedObject.FindProperty("info.tagType");
            m_IsTrigger= serializedObject.FindProperty("IsTrigger");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space(4);
            EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Shape"), target);
            EditorGUILayout.Space(4);
            
            // hitboxName
            string hitboxName = EditorGUILayout.TextField("Hitbox Name", m_hitboxName.stringValue);
            m_hitboxName.stringValue = hitboxName;
            
            // hitboxType
            HitboxType newType = (HitboxType)EditorGUILayout.EnumPopup("Hitbox Type", (HitboxType)m_hitboxType.enumValueIndex);
            m_hitboxType.enumValueIndex = (int)newType;
            
            // Tag
            TagType tagType = (TagType)EditorGUILayout.EnumPopup("Tag Type", (TagType)m_TagType.enumValueIndex);
            this.m_TagType.enumValueIndex = (int)tagType;
            
            //IsTrigger
            bool IsTrigger = EditorGUILayout.Toggle("IsTrigger",m_IsTrigger.boolValue);
            m_IsTrigger.boolValue = IsTrigger;
            
            EditorGUILayout.Space(4);
            EditorGUILayout.PropertyField(m_center);
            EditorGUILayout.PropertyField(m_size);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif