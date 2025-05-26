using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    public class RuntimePlayable
    {
        public long instanceId;
        public TimelinePlayer TimelinePlayer;
        public BBTimeline Timeline;
        public List<RuntimeTrack> RuntimeTracks = new();
        private int CurrentFrame = -1;
        
        public PlayableGraph PlayableGraph => TimelinePlayer.PlayableGraph;
        public AnimationLayerMixerPlayable AnimationRootPlayable => TimelinePlayer.AnimationRootPlayable;
        

        public static RuntimePlayable Create(BBTimeline _timeline, TimelinePlayer _timelinePlayer)
        {
            RuntimePlayable runtimePlayable = new();
            runtimePlayable.Timeline = _timeline;
            runtimePlayable.TimelinePlayer = _timelinePlayer;
            runtimePlayable.Init();
            runtimePlayable.RebindCallback += runtimePlayable.Rebind;
            return runtimePlayable;
        }

        //解耦合需要达成什么目标? 
        //1. TrackView 渲染完全脱离RuntimeTrack ,只跟数据层有关
        //2. TrackView中支持抛出事件到逻辑层
        //3. 支持选择是否evaluate track
        private void Init()
        {
            Timeline.Tracks.ForEach(track =>
            {
                if (!track.Enable) return;

                Type trackType = track.RuntimeTrackType;
                RuntimeTrack runtimeTrack = Activator.CreateInstance(trackType, this, track) as RuntimeTrack;
                runtimeTrack.Bind();
                RuntimeTracks.Add(runtimeTrack);
            });
        }

        public void Dispose()
        {
            CurrentFrame = -1;
            foreach (RuntimeTrack runtimeTrack in RuntimeTracks)
            {
                runtimeTrack.UnBind();
            }

            RuntimeTracks.Clear();
            TimelinePlayer.ClearTimelineGenerate();
        }

        public void Evaluate(int targetFrame)
        {
            //1. dont call each update 
            if (CurrentFrame == targetFrame) return;
            CurrentFrame = targetFrame;

            //2. mute runtimeTrack
            for (int i = RuntimeTracks.Count - 1; i >= 0; i--)
            {
                RuntimeTrack runtimeTrack = RuntimeTracks[i];
                runtimeTrack.SetTime(targetFrame);
            }

            //3. mute playable 
            PlayableGraph.Evaluate();
        }
        
        public BBTrack AddTrack(Type type)
        {
            BBTrack track = Timeline.AddTrack(type);
            return track;
        }

        public void RemoveTrack(BBTrack track)
        {
            Timeline.RemoveTrack(track);
        }
        
        public Action RebindCallback;

        private void Rebind()
        {
            Dispose();
            Init();
        }

        public int ClipMaxFrame()
        {
            int maxFrame = 0;
            foreach (BBTrack track in Timeline.Tracks)
            {
                if (maxFrame <= track.GetMaxFrame())
                {
                    maxFrame = track.GetMaxFrame();
                }
            }

            return maxFrame;
        }

        public int GetNow()
        {
            return CurrentFrame;
        }
    }

    public abstract class RuntimeTrack
    {
        protected RuntimeTrack(RuntimePlayable runtimePlayable, BBTrack track)
        {
            RuntimePlayable = runtimePlayable;
            Track = track;
        }
        
        public BBTrack Track;
        protected int PlayableIndex;
        protected RuntimePlayable RuntimePlayable;
        
        public abstract void Bind();
        public abstract void UnBind();
        public abstract void SetTime(int targetFrame);

        public int ClipCount => Track.Clips.Count;
    }
}