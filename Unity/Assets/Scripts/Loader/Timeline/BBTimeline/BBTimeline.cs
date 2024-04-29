using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline", fileName = "BBTimeline")]
    public class BBTimeline: SerializedScriptableObject
    {
        public List<BBTrack> Tracks = new();

#if UNITY_EDITOR
        [HideInInspector]
        public UnityEditor.SerializedObject SerializedTimeline;

        public void AddTrack(Type type)
        {
            BBTrack track = Activator.CreateInstance(type) as BBTrack;
            track.Name = type.Name.Replace("Track", string.Empty);
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

    public abstract class BBTrack
    {
        public string Name;
        public List<BBClip> Clips = new();

        public virtual Type RuntimeTrackType => typeof (RuntimeTrack);
        
#if UNITY_EDITOR
        protected virtual Type ClipType => typeof (Clip);

        public BBClip AddClip(int frame)
        {
            BBClip clip = Activator.CreateInstance(ClipType, frame) as BBClip;
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

    public abstract class BBClip
    {
        public bool InValid;

        public string Name => GetType().Name;

        public int StartFrame;
        public int EndFrame;
        public int Length => EndFrame - StartFrame;
        public ClipCapabilities Capabilities;

        public BBClip(int frame)
        {
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

        public bool IsResizeable()
        {
            return (Capabilities & ClipCapabilities.Resizeable) == ClipCapabilities.Resizeable;
        }

        public bool IsMixable()
        {
            return (Capabilities & ClipCapabilities.Mixable) == ClipCapabilities.Mixable;
        }
#endif
    }
}