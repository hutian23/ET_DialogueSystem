using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    public class RuntimePlayable
    {
        public BBTimeline Timeline;
        public TimelinePlayer TimelinePlayer;

        #region Component

        public Animator Animator => TimelinePlayer.GetComponent<Animator>();
        public AudioSource AudioSource => TimelinePlayer.GetComponent<AudioSource>();
        public PlayableGraph PlayableGraph => TimelinePlayer.PlayableGraph;
        public AnimationLayerMixerPlayable AnimationRootPlayable => TimelinePlayer.AnimationRootPlayable;
        public AudioMixerPlayable AudioRootPlayable => TimelinePlayer.AudioRootPlayable;

        #endregion

        public List<RuntimeTrack> RuntimeTracks = new();
        private int CurrentFrame;

        public static RuntimePlayable Create(BBTimeline _timeline, TimelinePlayer _timelinePlayer)
        {
            RuntimePlayable runtimePlayable = new();
            runtimePlayable.Timeline = _timeline;
            runtimePlayable.TimelinePlayer = _timelinePlayer;
            runtimePlayable.Init();
            runtimePlayable.RebindCallback += runtimePlayable.Rebind;

            return runtimePlayable;
        }

        private void Init()
        {
            Timeline.Tracks.ForEach(track =>
            {
                Type trackType = track.RuntimeTrackType;
                RuntimeTrack runtimeTrack = Activator.CreateInstance(trackType, this, track) as RuntimeTrack;
                runtimeTrack.Bind();
                RuntimeTracks.Add(runtimeTrack);
            });
        }

        public void Dispose()
        {
            CurrentFrame = 0;
            foreach (var runtimeTrack in RuntimeTracks)
            {
                runtimeTrack.UnBind();
            }

            RuntimeTracks.Clear();
            TimelinePlayer.ClearTimelineGenerate();
        }

        public void Evaluate(int targetFrame)
        {
            if (CurrentFrame == targetFrame) return;
            CurrentFrame = targetFrame;

            for (int i = RuntimeTracks.Count - 1; i >= 0; i--)
            {
                RuntimeTrack runtimeTrack = RuntimeTracks[i];
                runtimeTrack.SetTime(targetFrame);
            }

            PlayableGraph.Evaluate();
        }

#if UNITY_EDITOR
        public BBTrack AddTrack(Type type)
        {
            BBTrack track = Timeline.AddTrack(type);
            return track;
        }

        public void RemoveTrack(BBTrack track)
        {
            Timeline.RemoveTrack(track);
        }
#endif

        //TODO Rebind -- 对应Undo Redo
        public Action RebindCallback;

        private void Rebind()
        {
            Dispose();
            Init();
        }

        public int ClipMaxFrame()
        {
            int maxFrame = 0;
            foreach (var track in Timeline.Tracks)
            {
                foreach (var clip in track.Clips)
                {
                    if (clip.EndFrame >= maxFrame)
                    {
                        maxFrame = clip.EndFrame;
                    }
                }
            }

            return maxFrame;
        }
    }

    public abstract class RuntimeTrack
    {
        protected RuntimeTrack(RuntimePlayable runtimePlayable, BBTrack track)
        {
            RuntimePlayable = runtimePlayable;
            Track = track;
        }

        protected RuntimePlayable RuntimePlayable;
        public BBTrack Track;

        protected int PlayableIndex;
        public abstract void Bind();
        public abstract void UnBind();
        public abstract void SetTime(int targetFrame);
        public abstract void RuntimMute(bool value);

        public int ClipCount => Track.Clips.Count;
    }
}