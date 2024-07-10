using System;
using System.Collections.Generic;
using System.Linq;
using ET;
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

        public EventInfo GetInfo(int targetFrame)
        {
            return EventInfos.FirstOrDefault(info => info.frame == targetFrame);
        }

#if UNITY_EDITOR
        public override Type TrackViewType => typeof (EventTrackView);
        public override int GetMaxFrame()
        {
            return EventInfos.Count > 0? EventInfos.Max(info => info.frame) : 0;
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

    public struct EventCallback
    {
        public long instanceId;

        public EventInfo info;
    }

#if UNITY_EDITOR
    public class EventInspectorData: ShowInspectorData
    {
        [HideReferenceObjectPicker, HideLabel]
        public EventInfo Info;

        private TimelineFieldView FieldView;

        public EventInspectorData(object target): base(target)
        {
            Info = target as EventInfo;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }
#endif

    public class RuntimeEventTrack: RuntimeTrack
    {
        private int currentFrame = -1;
        private readonly BBEventTrack track;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        
        public RuntimeEventTrack(RuntimePlayable runtimePlayable, BBTrack _track): base(runtimePlayable, _track)
        {
            track = _track as BBEventTrack;
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
            if (currentFrame == targetFrame)
            {
                return;
            }

            currentFrame = targetFrame;

            EventInfo info = track.GetInfo(targetFrame);
            if (info != null)
            {
                EventSystem.Instance?.Invoke(new EventCallback());
            }
        }
    }
}