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

#if UNITY_EDITOR
    public class EventInspectorData: ShowInspectorData
    {
        [HideReferenceObjectPicker, HideLabel]
        public EventInfo Info;

        public EventInspectorData(object target): base(target)
        {
            Info = target as EventInfo;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }
#endif

    public struct EventMarkerCallback
    {
        public long instanceId;

        public EventInfo info;
    }

    public struct InitEventTrack
    {
        public long instanceId;
        public RuntimeEventTrack RuntimeEventTrack;
    }

    public class RuntimeEventTrack: RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        private int currentFrame = -1;

        public RuntimeEventTrack(RuntimePlayable runtimePlayable, BBTrack _track): base(runtimePlayable, _track)
        {
        }

        public override void Bind()
        {
            //Init component of event track
            EventSystem.Instance?.Invoke(new InitEventTrack() { instanceId = timelinePlayer.instanceId, RuntimeEventTrack = this });
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

            BBEventTrack eventTrack = Track as BBEventTrack;
            EventInfo targetInfo = eventTrack.GetInfo(currentFrame);
            if (targetInfo == null)
            {
                return;
            }

            //目前的想法是 跟 AnimationEvent保持一致， 同步调用动画帧事件(协程调用当前异步动画帧事件)
            EventSystem.Instance?.Invoke(new EventMarkerCallback() { instanceId = timelinePlayer.instanceId, info = targetInfo });
        }
    }
}