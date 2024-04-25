using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline", fileName = "BBTimeline")]
    public class BBTimeline: SerializedScriptableObject
    {
        [BsonIgnore]
        public string GraphName;

        public List<BBTrack> Tracks = new();

#if UNITY_EDITOR
        [HideInInspector]
        public UnityEditor.SerializedObject SerializedTimeline;

        public void AddTrack(Type type)
        {
            BBTrack track = Activator.CreateInstance(type) as BBTrack;

            track.Name = type.Name.Replace("Track", string.Empty);
            track.Timeline = this;

            Tracks.Add(track);
        }

        public void RemoveTrack(BBTrack track)
        {
            Tracks.Remove(track);
        }

        public BBClip AddClip(BBTrack track, int frame)
        {
            //检查重合
            foreach (BBClip _clip in track.Clips)
            {
                if (_clip.Contain(frame))
                {
                    Debug.LogError("Overlap with other clip!!!");
                    return null;
                }
            }

            return track.AddClip(frame);
        }

        public void RemoveClip(BBTrack track, BBClip clip)
        {
            track.RemoveClip(clip);
        }
#endif
    }

    public class BBTrack
    {
        public string Name;

        [HideInInspector, BsonIgnore]
        public BBTimeline Timeline;

        public List<BBClip> Clips = new();

#if UNITY_EDITOR
        protected virtual Type ClipType => typeof (Clip);

        public BBClip AddClip(int frame)
        {
            BBClip clip = Activator.CreateInstance(ClipType, this, frame) as BBClip;
            Clips.Add(clip);
            return clip;
        }

        public void RemoveClip(BBClip clip)
        {
            Clips.Remove(clip);
        }

        public Color Color()
        {
            var colorAttributes = GetType().GetCustomAttributes<ColorAttribute>().ToArray();
            return colorAttributes[^1].Color / 255;
        }

#endif
    }

    public class BBClip
    {
        public BBTrack Track;
        public BBTimeline Timeline => Track.Timeline;

        public bool InValid;

        public virtual string Name => GetType().Name;

        public int StartFrame;
        public int EndFrame;
        public virtual int Length => EndFrame - StartFrame;
        public ClipCapabilities Capabilities;

        public BBClip(BBTrack track, int frame)
        {
            Track = track;
            StartFrame = frame;
            EndFrame = StartFrame + 3;
        }

#if UNITY_EDITOR
        public bool Contain(float halfFrame)
        {
            return StartFrame < halfFrame && halfFrame < EndFrame;
        }

        public bool Overlap(BBClip clip)
        {
            for (int i = clip.StartFrame; i <= clip.EndFrame; i++)
            {
                if (Contain(i)) return true;
            }

            return false;
        }

        public Color Color()
        {
            var colorAttributes = GetType().GetCustomAttributes<ColorAttribute>().ToArray();
            return colorAttributes[^1].Color / 255;
        }

        public virtual bool IsResizeable()
        {
            return (Capabilities & ClipCapabilities.Resizeable) == ClipCapabilities.Resizeable;
        }

        public virtual bool IsMixable()
        {
            return (Capabilities & ClipCapabilities.Mixable) == ClipCapabilities.Mixable;
        }
#endif
    }
}