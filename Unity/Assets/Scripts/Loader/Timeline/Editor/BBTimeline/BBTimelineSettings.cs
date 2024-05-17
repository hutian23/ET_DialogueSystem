using UnityEngine;

namespace Timeline
{
    //跨场景复制数据可能需要
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/Setting", fileName = "BBTimelineSettings")]
    public class BBTimelineSettings: ScriptableObject
    {
        public static BBTimelineSettings GetSettings()
        {
            return Resources.Load<BBTimelineSettings>(nameof (BBTimelineSettings));
        }

        public GameObject hitboxPrefab;

        [Sirenix.OdinInspector.ReadOnly]
        public System.Object CopyTarget;
    }
}