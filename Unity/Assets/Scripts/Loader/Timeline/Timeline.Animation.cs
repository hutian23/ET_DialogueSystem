using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    [TrackGroup("Base")]
    [Ordered]
    [Color(127,253,228)]
    public sealed class AnimationTrack: Track
    {
        private static AvatarMask s_FullBodyMask;

        private static AvatarMask FullBodyMask
        {
            get
            {
                if (s_FullBodyMask == null)
                {
                    s_FullBodyMask = new AvatarMask();
                    for (int i = 0; i < 13; i++)
                    {
                        s_FullBodyMask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)i, true);
                    }
                }

                return s_FullBodyMask;
            }
        }

        [ShowInInspector, OnValueChanged("ReBindTimeline")]
        public AvatarMask AvatarMask;

        [ShowInInspector]
        public float EaseOutTime;

        [ShowInInspector]
        public bool PlayWhenEaseOut;

        public int PlayableIndex { get; protected set; }
        public TimelineAnimationTrackPlayable TrackPlayable { get; protected set; }
        public List<TimelineAnimationClipPlayable> ClipPlayables { get; protected set; }
        public event Action Delay;
        public int m_ExecutedCount;

        public void Executed()
        {
            m_ExecutedCount++;
            if (m_ExecutedCount == Clips.Count)
            {
                m_ExecutedCount = 0;
                Delay?.Invoke();
                Delay = null;
            }
        }

        public override void Evaluate(float deltaTime)
        {
        }

        public override void Bind()
        {
            TrackPlayable = TimelineAnimationTrackPlayable.Create(this, Timeline.AnimationRootPlayable);
            PlayableIndex = Timeline.AnimationRootPlayable.GetInputCount() - 1;
            ClipPlayables = new List<TimelineAnimationClipPlayable>();

            if (m_PersistentMuted)
            {
                Timeline.AnimationRootPlayable.SetInputWeight(PlayableIndex, 0);
                return;
            }

            for (int i = 0; i < Clips.Count; i++)
            {
                ClipPlayables.Add(TimelineAnimationClipPlayable.Create(Clips[i] as AnimationClip, TrackPlayable.MixerPlayable, i));
            }

            if (AvatarMask)
            {
                Timeline.AnimationRootPlayable.SetLayerMaskFromAvatarMask((uint)PlayableIndex, AvatarMask);
            }
            else
            {
                Timeline.AnimationRootPlayable.SetLayerMaskFromAvatarMask((uint)PlayableIndex, FullBodyMask);
            }
        }

        public override void UnBind()
        {
            if (TrackPlayable != null)
            {
                if (!Application.isPlaying || EaseOutTime == 0)
                {
                    Timeline.AnimationRootPlayable.DisconnectInput(PlayableIndex);
                    TrackPlayable.Handle.Destroy();
                }
            }
            else if (Timeline.Time < Timeline.Duration)
            {
                Timeline.TimelinePlayer.AddAnimationEaseOut(this);
            }
        }

        public override void SetTime(float time)
        {
            TrackPlayable.SetTime(time);
            ClipPlayables.ForEach(x => x.SetTime(time));
        }

        protected float m_OriginalWeight;

        public override void RuntimeMute(bool value)
        {
            if (PersistentMuted)
            {
                return;
            }

            if (value && !RuntimeMuted)
            {
                m_OriginalWeight = Timeline.AnimationRootPlayable.GetInputWeight(PlayableIndex);
                RuntimeMuted = true;
                Timeline.AnimationRootPlayable.SetInputWeight(PlayableIndex, value? 0 : 1);
            }
            else if (!value && RuntimeMuted)
            {
                RuntimeMuted = false;
                Timeline.AnimationRootPlayable.SetInputWeight(PlayableIndex, m_OriginalWeight);
            }
        }

#if UNITY_EDITOR
        public override Type ClipType => typeof (AnimationClip);

        public override Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            AnimationClip clip = new AnimationClip(referenceObject as UnityEngine.AnimationClip, this, frame);
            m_Clips.Add(clip);
            return clip;
        }

        public override bool DragValid()
        {
            return UnityEditor.DragAndDrop.objectReferences.Length == 1 && UnityEditor.DragAndDrop.objectReferences[0] as UnityEngine.AnimationClip;
        }
#endif
    }

    public class TimelineAnimationTrackPlayable: PlayableBehaviour
    {
        public AnimationTrack Track { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AnimationMixerPlayable MixerPlayable { get; private set; }
        public Timeline Timeline => Track.Timeline;

        //PrepareFrame should be used to do topological modifications, change connection weights, time changes , etc.
        public override void PrepareFrame(Playable playable, FrameData data)
        {
            Track.Delay += () =>
            {
                if (Track.RuntimeMuted)
                {
                    return;
                }

                float sumWeight = 0;
                foreach (var clipPlayable in Track.ClipPlayables)
                {
                    sumWeight += clipPlayable.TargetWeight;
                }

                // float weight = Mathf.Clamp01(sumWeight);
                if (sumWeight == 0)
                {
                    Output.SetInputWeight(Track.PlayableIndex, 0);
                }
                else if (0 < sumWeight && sumWeight < 1)
                {
                    Output.SetInputWeight(Track.PlayableIndex, sumWeight);
                }
                else
                {
                    Output.SetInputWeight(Track.PlayableIndex, 1);
                }
            };
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            MixerPlayable.SetTime(time);
            PrepareFrame(default, default);
        }

        public static TimelineAnimationTrackPlayable Create(AnimationTrack track, Playable output)
        {
            var handle = ScriptPlayable<TimelineAnimationTrackPlayable>.Create(track.Timeline.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.Track = track;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AnimationMixerPlayable.Create(track.Timeline.PlayableGraph, track.Clips.Count);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0, 0);
            return trackPlayable;
        }
    }

    public class AnimationClip: Clip
    {
        [ShowInInspector, OnValueChanged("OnClipChanged", "RebindTimeline")]
        public UnityEngine.AnimationClip Clip;

        [ShowInInspector, OnValueChanged("RebindTimeline")]
        public ExtraPolationMode ExtraPolationMode;

#if UNITY_EDITOR
        public override string Name => Clip? Clip.name : base.Name;
        public override int Length => Clip? Mathf.RoundToInt(Clip.length * TimelineUtility.FrameRate) : base.Length;
        public override ClipCapabilities Capabilities => ClipCapabilities.Resizeable | ClipCapabilities.Mixable | ClipCapabilities.ClipInable;

        public AnimationClip(Track track, int frame): base(track, frame)
        {
        }

        public AnimationClip(UnityEngine.AnimationClip clip, Track track, int frame): base(track, frame)
        {
            Clip = clip;
            EndFrame = Length + frame;
        }

        protected void OnClipChanged()
        {
            OnNameChanged?.Invoke();
        }
#endif
    }

    public class TimelineAnimationClipPlayable: PlayableBehaviour
    {
        public AnimationClip Clip { get; private set; }
        public AnimationTrack Track => Clip.Track as AnimationTrack;
        public int Index { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AnimationClipPlayable ClipPlayable { get; private set; }
        public float TargetWeight { get; private set; }

        protected float m_LastTime;
        protected float m_HandleTime;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            m_HandleTime = (float)Handle.GetTime();
            float deltaTime = info.deltaTime;

            TimelineUtility.Lerp(m_HandleTime, deltaTime, Evaluate, ref m_LastTime);
            Track.Executed();
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            TimelineUtility.Lerp(time, time, Evaluate, ref m_LastTime);
            Track.Executed();
        }

        public void Evaluate(float deltaTime)
        {
            if (m_LastTime < Clip.StartTime)
            {
                TargetWeight = 0;
                Output.SetInputWeight(Index, TargetWeight);
                ClipPlayable.SetTime(0);
            }
            else if (Clip.StartTime <= m_LastTime && m_LastTime <= Clip.EndTime)
            {
                float selfTime = m_LastTime - Clip.StartTime;
                float remainTime = Clip.EndTime - m_LastTime;
                ClipPlayable.SetTime(selfTime + Clip.ClipInTime);

                if (selfTime < Clip.EaseInTime)
                {
                    TargetWeight = selfTime / Clip.EaseInTime;
                    if (Clip.OtherEaseInTime > 0)
                    {
                        Output.SetInputWeight(Index, TargetWeight);
                    }
                    else
                    {
                        Output.SetInputWeight(Index, 1);
                    }
                }
                else if (remainTime < Clip.EaseOutTime)
                {
                    TargetWeight = remainTime / Clip.EaseOutTime;
                    if (Clip.OtherEaseOutTime > 0)
                    {
                        Output.SetInputWeight(Index, TargetWeight);
                    }
                    else
                    {
                        Output.SetInputWeight(Index, 1);
                    }
                }
                else
                {
                    TargetWeight = 1;
                    Output.SetInputWeight(Index, TargetWeight);
                }
            }
            else if (m_LastTime > Clip.EndTime)
            {
                ClipPlayable.SetTime(Clip.DurationTime + Clip.ClipInTime);
                switch (Clip.ExtraPolationMode)
                {
                    case ExtraPolationMode.None:
                        TargetWeight = 0;
                        Output.SetInputWeight(Index, TargetWeight);
                        break;
                    case ExtraPolationMode.Hold:
                        //Keep?
                        break;
                }
            }
        }

        public static TimelineAnimationClipPlayable Create(AnimationClip clip, Playable output, int index)
        {
            var handle = ScriptPlayable<TimelineAnimationClipPlayable>.Create(clip.Timeline.PlayableGraph);
            var clipPlayable = handle.GetBehaviour();
            clipPlayable.Clip = clip;
            clipPlayable.Handle = handle;
            clipPlayable.ClipPlayable = AnimationClipPlayable.Create(clip.Timeline.PlayableGraph, clip.Clip);
            clipPlayable.ClipPlayable.SetApplyFootIK(false);
            handle.AddInput(clipPlayable.ClipPlayable, 0, 1);

            clipPlayable.Output = output;
            clipPlayable.Index = index;
            output.ConnectInput(index, handle, 0, 0);

            return clipPlayable;
        }
    }
}