using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("Hitbox")]
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
    public class BBHitboxTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeHitboxTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBHitboxClip);
        public override Type ClipViewType => typeof (HitboxClipView);
#endif
    }

    [Serializable]
    public struct HitboxInfo
    {
        public Rect hitBoxRect;
    }

    [Color(165, 032, 025)]
    public class BBHitboxClip: BBClip
    {
        [DictionaryDrawerSettings(KeyLabel = "Frame", ValueLabel = "HitboxInfo")]
        public Dictionary<int, HitboxInfo> hitboxInfos;

        public BBHitboxClip(int frame): base(frame)
        {
            hitboxInfos = new Dictionary<int, HitboxInfo>();
        }
    }

    #region Runtime

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
        }

        public override void SetTime()
        {
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion
}