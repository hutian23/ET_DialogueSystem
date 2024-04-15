using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    // [TrackGroup("Base")]
    [Ordered(1)]
    [Color(255,193,7)]
    public class AudioTrack: Track
    {
        public int PlayableIndex { get; protected set; }
        public TimelineAudioTrackPlayable TrackPlayable { get; protected set; }
        public List<TimelineAudioClipPlayable> ClipPlayables { get; protected set; }
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
            TrackPlayable = TimelineAudioTrackPlayable.Create(this, Timeline.AudioRootPlayable);
            PlayableIndex = Timeline.AudioRootPlayable.GetInputCount() - 1;
            ClipPlayables = new List<TimelineAudioClipPlayable>();

            if (m_PersistentMuted)
            {
                Timeline.AnimationRootPlayable.SetInputWeight(PlayableIndex, 0);
                return;
            }

            for (int i = 0; i < Clips.Count; i++)
            {
                ClipPlayables.Add(TimelineAudioClipPlayable.Create(Clips[i] as AudioClip, TrackPlayable.MixerPlayable, i));
            }
        }

        public override void UnBind()
        {
            if (TrackPlayable != null)
            {
                Timeline.AudioRootPlayable.DisconnectInput(PlayableIndex);
                TrackPlayable.Handle.Destroy();

                TrackPlayable = null;
                Delay = null;
            }
        }

        public override void SetTime(float time)
        {
            TrackPlayable.SetTime(time);
            // ClipPlayables.ForEach(x => k);
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
                m_OriginalWeight = Timeline.AudioRootPlayable.GetInputWeight(PlayableIndex);
                RuntimeMuted = true;
            }
            else if (!value && RuntimeMuted)
            {
                RuntimeMuted = false;
                Timeline.AudioRootPlayable.SetInputWeight(PlayableIndex, m_OriginalWeight);
            }
        }
#if UNITY_EDITOR
        public override Type ClipType => typeof (AudioClip);

        public override Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            AudioClip clip = new AudioClip(referenceObject as UnityEngine.AudioClip, this, frame);
            m_Clips.Add(clip);
            return clip;
        }

        public override bool DragValid()
        {
            return UnityEditor.DragAndDrop.objectReferences.Length == 1 && UnityEditor.DragAndDrop.objectReferences[0] as UnityEngine.AudioClip;
        }
#endif
    }

    public class TimelineAudioTrackPlayable: PlayableBehaviour
    {
        public AudioTrack Track { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AudioMixerPlayable MixerPlayable { get; private set; }
        public Timeline Timeline => Track.Timeline;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            MixerPlayable.SetTime(time);
            PrepareFrame(default, default);
        }

        public static TimelineAudioTrackPlayable Create(AudioTrack track, Playable output)
        {
            var handle = ScriptPlayable<TimelineAudioTrackPlayable>.Create(track.Timeline.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.Track = track;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AudioMixerPlayable.Create(track.Timeline.PlayableGraph, track.Clips.Count);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0, 1);

            return trackPlayable;
        }
    }

    public class AudioClip: Clip
    {
        [ShowInInspector, OnValueChanged("OnClipChanged", "ReBindTimeline")]
        public UnityEngine.AudioClip Clip;

        [ShowInInspector, OnValueChanged("ReBindTimeline")]
        public float Speed = 1;

#if UNITY_EDITOR
        public override string Name => Clip? Clip.name : base.Name;
        public override int Length => Clip? Mathf.RoundToInt(Clip.length / Speed * TimelineUtility.FrameRate) : base.Length;
        public override ClipCapabilities Capabilities => ClipCapabilities.Resizeable | ClipCapabilities.Mixable | ClipCapabilities.ClipInable;

        public AudioClip(Track track, int frame): base(track, frame)
        {
        }

        public AudioClip(UnityEngine.AudioClip clip, Track track, int frame): base(track, frame)
        {
            Clip = clip;
            EndFrame = Length + frame;
        }

        public void OnClipChanged()
        {
            OnNameChanged?.Invoke();
        }
#endif
    }

    public class TimelineAudioClipPlayable: PlayableBehaviour
    {
        public AudioClip Clip { get; private set; }
        public AudioTrack Track => Clip.Track as AudioTrack;

        public int Index { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AudioClipPlayable ClipPlayable { get; private set; }
        public float TargetWeight { get; private set; }

        protected float m_LastTime;
        protected float m_HandleTime;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            m_HandleTime = (float)Handle.GetTime();
            float deltaTime = info.deltaTime * Clip.Speed;
            m_LastTime += deltaTime;
        }

        public void SetTime(float time)
        {
            Handle.SetTime(time);
            m_LastTime += time;
            Evaluate(time);
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
                ClipPlayable.Seek(selfTime * Clip.ClipInTime, 0, Clip.DurationTime);

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
                TargetWeight = 0;
                Output.SetInputWeight(Index, TargetWeight);
                ClipPlayable.SetTime(Clip.DurationTime + Clip.ClipInTime);
            }
        }

        public static TimelineAudioClipPlayable Create(AudioClip clip, Playable output, int index)
        {
            var handle = ScriptPlayable<TimelineAudioClipPlayable>.Create(clip.Timeline.PlayableGraph);

            var clipPlayable = handle.GetBehaviour();
            clipPlayable.Clip = clip;
            clipPlayable.Handle = handle;
            clipPlayable.ClipPlayable = AudioClipPlayable.Create(clip.Timeline.PlayableGraph, clip.Clip, false);
            clipPlayable.ClipPlayable.SetDuration(clip.Duration / clip.Speed);

            handle.AddInput(clipPlayable.ClipPlayable, 0, 1);

            clipPlayable.Output = output;
            clipPlayable.Index = index;
            output.ConnectInput(index, handle, 0, 1);

            return clipPlayable;
        }
    }
}