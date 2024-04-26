using System;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    public class BBAnimationTrack: BBTrack
    {
        protected override Type ClipType => typeof (BBAnimationClip);
    }

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
        public override void Bind()
        {
            
        }

        public override void UnBind()
        {
            throw new NotImplementedException();
        }

        public override void SetTime()
        {
            throw new NotImplementedException();
        }

        public override void RuntimMute(bool value)
        {
            throw new NotImplementedException();
        }
    }
    
    public class BBTimelineAnimationTrackPlayable: PlayableBehaviour
    {
        private BBRuntimePlayable runtimePlayable;
        private BBAnimationTrack Track { get; set; }
        private Playable Output { get; set; }
        private Playable Handle { get; set; }
        private AnimationMixerPlayable MixerPlayable { get; set; }

        public static BBTimelineAnimationTrackPlayable Create(BBRuntimePlayable runtimePlayable, BBAnimationTrack track, Playable output)
        {
            var handle = ScriptPlayable<BBTimelineAnimationTrackPlayable>.Create(runtimePlayable.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.Track = track;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AnimationMixerPlayable.Create(runtimePlayable.PlayableGraph, track.Clips.Count);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0, 0);
            return trackPlayable;
        }
    }
    
    
    #endregion
}