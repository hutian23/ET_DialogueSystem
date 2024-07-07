using UnityEditor;
using UnityEngine;

namespace Timeline.Editor
{
    //跨场景复制数据可能需要
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/Setting", fileName = "BBTimelineSettings")]
    public class BBTimelineSettings: ScriptableObject
    {
        public static BBTimelineSettings GetSettings()
        {
            return Resources.Load<BBTimelineSettings>(nameof (BBTimelineSettings));
        }

        [Sirenix.OdinInspector.ReadOnly]
        public System.Object CopyTarget;

        public BehaviorActiveObject BehaviorActiveObject;

        public void SetActiveObject(System.Object activeObject)
        {
            BehaviorActiveObject.ActiveObject = activeObject;
            Selection.activeObject = BehaviorActiveObject;
        }
        
        public void OnEnable()
        {
            BehaviorActiveObject = CreateInstance<BehaviorActiveObject>();
        }
    }
}