using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
    [Serializable]
    public class HitboxClipInfo
    {
        [HideInInspector]
        public BBHitboxClip clip;

        [LabelText("当前帧")]
        public int Frame;

        [LabelText("绑定对象")]
        public GameObject bindGo;

        [ButtonGroup("绑定GameObject")]
        public void Bind()
        {
        }

        // [Space(10)]
        // [ListDrawerSettings(IsReadOnly = true)]
        // public List<HitboxInfo> HitboxInfos = new();
    }

    public class HitboxClipView: TimelineClipView
    {
    }

    public class HitboxInspectorView: SerializedScriptableObject
    {
        [HideReferenceObjectPicker]
        public HitboxClipInfo info;
    }
}