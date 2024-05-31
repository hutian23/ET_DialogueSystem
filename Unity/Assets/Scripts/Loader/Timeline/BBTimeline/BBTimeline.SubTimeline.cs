using System;
using Timeline.Editor;

namespace Timeline
{
    // Timeline 记录keyframe
    // keyframe应该包含什么信息?
    // 1. Sprite -- 对应AnimationClip
    // 2. Hitbox
    // 3. targetbind -- Vector3
    // 4. particle -- position rotation
    // 5. sound 是否需要添加到keyframe中？ 感觉不需要

    [BBTrack("SubTimeline")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("799823b53d556d34faeb55e049c91845")]
#endif
    public class SubTimelineTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeSubTimelineTrack);

#if UNITY_EDITOR
        protected override Type ClipType => typeof (SubTimelineClip);
        public override Type ClipViewType => typeof (SubTimelineClipView);
#endif
    }

    [Color(100, 100, 100)]
    public class SubTimelineClip: BBClip
    {
        public SubTimelineClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBSubTimelineInspectorData);
#endif
    }

    #region Editor

    public class BBSubTimelineInspectorData: ShowInspectorData
    {
        private TimelineFieldView fieldView;
        private SubTimelineClip subTimelineClip;

        public BBSubTimelineInspectorData(object target): base(target)
        {
            subTimelineClip = target as SubTimelineClip;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }

    #endregion

    #region Runtime

    public class RuntimeSubTimelineTrack: RuntimeTrack
    {
        public RuntimeSubTimelineTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
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

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion
}