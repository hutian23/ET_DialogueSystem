using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    public class RuntimePlayable
    {
        public BBTimeline timeline;
        public TimelinePlayer timelinePlayer;
        public PlayableGraph playableGraph;
        public AnimationLayerMixerPlayable animationRootPlayable;
     
        public List<RuntimeTrack> runtimeTracks = new();
        private int currentFrame;
        private long instanceId;
        
        public static RuntimePlayable Create(BBTimeline _timeline, TimelinePlayer _timelinePlayer)
        {
            // 创建PlayableGraph
            RuntimePlayable runtimePlayable = new();
            runtimePlayable.timeline = _timeline;
            runtimePlayable.timelinePlayer = _timelinePlayer;
            runtimePlayable.playableGraph = PlayableGraph.Create(_timeline.timelineName);
                
            // 动画混合
            runtimePlayable.animationRootPlayable = AnimationLayerMixerPlayable.Create(runtimePlayable.playableGraph);
            Animator animator = runtimePlayable.timelinePlayer.GetComponent<Animator>();
            AnimationPlayableOutput animationOutput = AnimationPlayableOutput.Create(runtimePlayable.playableGraph, "Animation", animator);
            animationOutput.SetSourcePlayable(runtimePlayable.animationRootPlayable);        
            
            // 创建轨道
            _timeline.Tracks.ForEach(track =>
            {
                if (!track.Enable) return;
                Type trackType = track.RuntimeTrackType;
                RuntimeTrack runtimeTrack = Activator.CreateInstance(trackType, runtimePlayable, track) as RuntimeTrack;
                runtimeTrack.Bind();
                runtimePlayable.runtimeTracks.Add(runtimeTrack);
            });
            runtimePlayable.currentFrame = -1;
            runtimePlayable.instanceId = _timelinePlayer.instanceId;
            
            return runtimePlayable;
        }

        public void Dispose()
        {
            // 销毁PlayableGraph
            runtimeTracks.ForEach(runtimeTrack => runtimeTrack.UnBind());
            runtimeTracks.Clear();
            if (playableGraph.IsValid())
            {
                animationRootPlayable.Destroy();
                playableGraph.Destroy();
            }
            
            timeline = null;
            timelinePlayer = null;
            currentFrame = -1;
            instanceId = 0;
        }

        public void Rebind()
        {
            runtimeTracks.ForEach(runtimeTrack => runtimeTrack.UnBind());
            runtimeTracks.Clear();
            timeline.Tracks.ForEach(track =>
            {
                if (!track.Enable) return;
                Type trackType = track.RuntimeTrackType;
                RuntimeTrack runtimeTrack = Activator.CreateInstance(trackType, this, track) as RuntimeTrack;
                runtimeTrack.Bind();
                runtimeTracks.Add(runtimeTrack);
            });
            currentFrame = -1;
        }
        
        public void Evaluate(int targetFrame)
        {
            //1. dont call each update 
            if (currentFrame == targetFrame) return;
            currentFrame = targetFrame;

            //2. mute runtimeTrack
            for (int i = 0; i < runtimeTracks.Count; i++)
            {
                RuntimeTrack runtimeTrack = runtimeTracks[i];
                runtimeTrack.SetTime(targetFrame);
            }

            //3. mute playable 
            playableGraph.Evaluate();
        }

        public bool HasBindUnit()
        {
            return instanceId != 0;
        }
        
        public BBTrack AddTrack(Type type)
        {
            BBTrack track = timeline.AddTrack(type);
            return track;
        }

        public void RemoveTrack(BBTrack track)
        {
            this.timeline.RemoveTrack(track);
        }

        public int GetMaxFrame()
        {
            int maxFrame = 0;
            foreach (BBTrack track in this.timeline.Tracks)
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
            return currentFrame;
        }

        public string GetKeyFrame(int targetFrame)
        {
            foreach (RuntimeTrack runtimeTrack in runtimeTracks)
            {
                if (runtimeTrack.Track is not BBEventTrack eventTrack) continue;
                if (eventTrack.Name.Equals("Marker"))
                {
                    EventInfo info = eventTrack.GetInfo(targetFrame);
                    return info == null? string.Empty : info.keyframeName;
                }
            }

            return string.Empty;
        }
        
        public long GetInstanceId()
        {
            return instanceId;
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