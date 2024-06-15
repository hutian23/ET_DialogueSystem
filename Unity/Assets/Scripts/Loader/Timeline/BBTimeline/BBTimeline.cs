using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Timeline
{
    [Serializable]
    public class MarkerInfo
    {
        [HideInInspector]
        public int frame;

        public string markerName;

        [FoldoutGroup("脚本: ")]
        [TextArea(14, 30), HideLabel]
        public string Script;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/Timeline", fileName = "BBTimeline")]
    public class BBTimeline: SerializedScriptableObject
    {
        public string timelineName = "New BBTimeline";

        [NonSerialized, OdinSerialize]
        public List<BBTrack> Tracks = new();

        [NonSerialized, OdinSerialize]
        public List<MarkerInfo> Marks = new();

#if UNITY_EDITOR
        [HideInInspector]
        public SerializedObject SerializedTimeline;

        public void UpdateSerializeTimeline()
        {
            SerializedTimeline = new SerializedObject(this);
        }

        public BBTrack AddTrack(Type type)
        {
            BBTrack track = Activator.CreateInstance(type) as BBTrack;
            track.Name = type.Name.Replace("Track", string.Empty);
            Tracks.Add(track);
            return track;
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

    [Serializable]
    public class BBTimelineKeyframe
    {
#if UNITY_EDITOR
        public int frame;
#endif
        public int a = 10;
    }

    public abstract class BBTrack
    {
        public string Name;

        [OdinSerialize, NonSerialized]
        public List<BBClip> Clips = new();

        public virtual Type RuntimeTrackType => typeof (RuntimeTrack);

#if UNITY_EDITOR
        protected virtual Type ClipType => typeof (Clip);
        public virtual Type ClipViewType => typeof (TimelineClipView);

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
#endif
    }

    public abstract class BBClip
    {
        public bool InValid;

        public string Name;

        public int StartFrame;
        public int EndFrame;
        public int Length => EndFrame - StartFrame;

        protected BBClip()
        {
            Name = GetType().Name;
        }

        protected BBClip(int frame)
        {
            StartFrame = frame;
            EndFrame = StartFrame + 3;
        }
#if UNITY_EDITOR
        public bool Contain(float halfFrame)
        {
            return StartFrame < halfFrame && halfFrame < EndFrame;
        }

        public bool InMiddle(int frame)
        {
            return StartFrame <= frame && frame <= EndFrame;
        }

        public bool Overlap(BBClip clip)
        {
            for (int i = clip.StartFrame; i <= clip.EndFrame; i++)
            {
                if (Contain(i)) return true;
            }

            return false;
        }

        public virtual Type ShowInInpsectorType => typeof (ShowInspectorData);
#endif
    }
}