using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using Timeline.Editor;
#endif
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    [Serializable]
    [BBTrack("Animation")]
#if UNITY_EDITOR
    [Color(127, 253, 228)]
    [IconGuid("46d1be470ea7f7945b52ec8511f9a419")]
#endif
    public class BBAnimationTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeAnimationTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBAnimationClip);
        public override Type ClipViewType => typeof (TimelineClipView);
#endif
    }

    [Color(127, 253, 228)]
    public class BBAnimationClip: BBClip
    {
        public UnityEngine.AnimationClip animationClip;

        public BBAnimationClip(int frame): base(frame)
        {
        }
    }

    #region Runtime

    public class RuntimeAnimationTrack: RuntimeTrack
    {
        public BBAnimationTrack AnimationTrack => Track as BBAnimationTrack;

        private BBTimelineAnimationTrackPlayable TrackPlayable;
        private AnimationMixerPlayable MixerPlayable => TrackPlayable.MixerPlayable;
        private readonly List<BBTimelineAnimationClipPlayable> ClipPlayables = new();

        public override void Bind()
        {
            TrackPlayable = BBTimelineAnimationTrackPlayable.Create(RuntimePlayable, this, RuntimePlayable.AnimationRootPlayable);
            PlayableIndex = RuntimePlayable.AnimationRootPlayable.GetInputCount() - 1;

            ClipPlayables.Clear();
            for (int i = 0; i < AnimationTrack.Clips.Count; i++)
            {
                var clipPlayable =
                        BBTimelineAnimationClipPlayable.Create(RuntimePlayable, AnimationTrack.Clips[i] as BBAnimationClip, MixerPlayable, i);
                ClipPlayables.Add(clipPlayable);
            }
        }

        public override void UnBind()
        {
            for (int i = 0; i < ClipPlayables.Count; i++)
            {
                BBTimelineAnimationClipPlayable clipPlayable = ClipPlayables[i];
                // MixerPlayable.DisconnectInput(i);
                // clipPlayable.Handle.Destroy();
            }

            // 取消连接关系并且销毁
            RuntimePlayable.AnimationRootPlayable.DisconnectInput(PlayableIndex);
            // TrackPlayable.Handle.Destroy();
        }

        public override void SetTime()
        {
        }

        public override void RuntimMute(bool value)
        {
        }

        public RuntimeAnimationTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }
    }

    public class BBTimelineAnimationTrackPlayable: PlayableBehaviour
    {
        private RuntimePlayable runtimePlayable;
        private BBAnimationTrack Track { get; set; }
        private Playable Output { get; set; }
        public Playable Handle { get; set; }
        public AnimationMixerPlayable MixerPlayable { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public static BBTimelineAnimationTrackPlayable Create(RuntimePlayable runtimePlayable, RuntimeAnimationTrack runtimeAnimationTrack,
        Playable output)
        {
            var handle = ScriptPlayable<BBTimelineAnimationTrackPlayable>.Create(runtimePlayable.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.Track = runtimeAnimationTrack.AnimationTrack;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AnimationMixerPlayable.Create(runtimePlayable.PlayableGraph, runtimeAnimationTrack.ClipCount);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0, 0);
            return trackPlayable;
        }
    }

    public class BBTimelineAnimationClipPlayable: PlayableBehaviour
    {
        private BBAnimationTrack Track { get; set; }
        private BBClip Clip { get; set; }

        public int Index { get; private set; }

        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        private AnimationClipPlayable ClipPlayable { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public void SetTime(float time)
        {
        }

        public void Evaluate(float deltaTime)
        {
        }

        public static BBTimelineAnimationClipPlayable Create(RuntimePlayable runtimePlayable, BBAnimationClip clip, Playable output, int index)
        {
            var handle = ScriptPlayable<BBTimelineAnimationClipPlayable>.Create(runtimePlayable.PlayableGraph);
            var clipPlayable = handle.GetBehaviour();
            clipPlayable.Clip = clip;
            clipPlayable.Handle = handle;
            clipPlayable.ClipPlayable = AnimationClipPlayable.Create(runtimePlayable.PlayableGraph, clip.animationClip);
            handle.AddInput(clipPlayable.ClipPlayable, 0, 1);

            clipPlayable.Output = output;
            clipPlayable.Index = index;
            output.ConnectInput(index, handle, 0, 0);

            return clipPlayable;
        }
    }

    #endregion
}