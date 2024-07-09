using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("Event")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("423519931448cce4b8c8320e4f526f3b")]
#endif
    public class BBEventTrack: BBTrack
    {
        [NonSerialized, OdinSerialize]
        public List<EventInfo> EventInfos = new();

        public override Type RuntimeTrackType => typeof (RuntimeEventTrack);
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (EventTrackView);
        public override int GetMaxFrame()
        {
            return EventInfos.Max(info => info.frame);
        }
#endif
    }

    [Serializable]
    public class EventInfo: BBKeyframeBase
    {
        [Title("Script")]
        [TextArea(14, 30), HideLabel]
        public string Script;
    }

    public class RuntimeEventTrack: RuntimeTrack
    {
        public RuntimeEventTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
        }
    }
}