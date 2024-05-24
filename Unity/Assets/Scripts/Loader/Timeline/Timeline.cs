using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [AcceptableTrackGroups("Base")]
    public partial class Timeline: ScriptableObject
    {
        [LabelText("行为名")]
        public string GraphName;

        [Space(10)]
        [SerializeReference]
        protected List<Track> m_Tracks = new();

        public List<Track> Tracks => m_Tracks;
        
        public event Action OnBindStateChanged;
        public event Action OnValueChanged;

        private int m_Frame;

        public int Frame
        {
            get => m_Frame;
            set
            {
                m_Frame = value;
            }
        }

        // private float m_Time;
        //
        // public float Time
        // {
        //     get => m_Time;
        //     set
        //     {
        //         m_Time = value; 
        //         OnEvaluated?.Invoke();
        //     }
        // }

        // public int Frame => Mathf.RoundToInt(Time * TimelineUtility.FrameRate);
        public int MaxFrame { get; protected set; }
        // public float Duration { get; private set; }

        private bool m_Binding;

        public bool Binding
        {
            get => m_Binding;
            protected set => m_Binding = value;
        }

        private TimelinePlayer m_TimelinePlayer;

        public TimelinePlayer TimelinePlayer
        {
            get => m_TimelinePlayer;
            protected set => m_TimelinePlayer = value;
        }

        public PlayableGraph PlayableGraph { get; protected set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; protected set; }
        public AudioMixerPlayable AudioRootPlayable { get; protected set; }

        public void Init()
        {
            if (TimelinePlayer != null)
            {
                TimelinePlayer.Dispose();
            }

            //1. Tracks
            MaxFrame = 0;
            m_Tracks.ForEach(track =>
            {
                track.Init(this);
                if (track.MaxFrame > MaxFrame)
                {
                    MaxFrame = track.MaxFrame;
                }
            });

            //2. AddTimeline
            TimelinePlayer.Init();
            TimelinePlayer.BindTimeline(this);
        }

        public void Evaluate(float deltaTime)
        {
            // Time += deltaTime;
            // Tracks.ForEach(t => { t.Evaluate(deltaTime); });
            // if (Time > Duration)
            // {
            //     OnDone?.Invoke();
            //     OnDone = null;
            // }
        }

        public void Evaluate(int deltaFrame)
        {
        }

        public void Bind(TimelinePlayer timelinePlayer)
        {
            Frame = 0;
            PlayableGraph = timelinePlayer.PlayableGraph;
            AnimationRootPlayable = timelinePlayer.AnimationRootPlayable;
            AudioRootPlayable = timelinePlayer.AudioRootPlayable;
            
            OnValueChanged += RebindAll;

            // m_Tracks.ForEach(t => t.Bind());
            //OnBindStateChanged?.Invoke();
        }

        public void UnBind()
        {
            OnValueChanged -= RebindAll;

            AnimationRootPlayable = AnimationLayerMixerPlayable.Null;
            AudioRootPlayable = default;
            PlayableGraph = default;
            TimelinePlayer = null;

            OnBindStateChanged?.Invoke();
        }

        public void JumpTo(float targetTime)
        {
            // float deltaTime = targetTime - Time;
            // TimelinePlayer.AddtionalDelta = deltaTime;
        }

        public void RebindAll()
        {
            // if (Binding)
            // {
            //     OnRebind?.Invoke();
            //     OnRebind = null;
            //
            //     foreach (var track in m_Tracks)
            //     {
            //         track.ReBind();
            //         track.SetTime(Time);
            //     }
            //
            //     TimelinePlayer.Evaluate(0);
            // }
        }

        public void RebindTrack(Track track)
        {
            // if (this.Binding)
            // {
            //     track.ReBind();
            //     track.SetTime(this.Time);
            //     this.TimelinePlayer.Evaluate(0);
            // }
        }

        public void RuntimeMute(int index, bool value)
        {
            if (0 <= index && index < this.m_Tracks.Count)
            {
                RuntimeMute(this.m_Tracks[index], value);
            }
        }

        public void RuntimeMute(Track track, bool value)
        {
            track.RuntimeMute(value);
        }
    }

    [Serializable]
    public abstract partial class Track
    {
        public string Name;

        [SerializeField]
        protected bool m_PersistentMuted;

        public bool PersistentMuted
        {
            get => m_PersistentMuted;
            set
            {
                if (m_PersistentMuted != value)
                {
                    m_PersistentMuted = value;
                    OnMutedStateChanged?.Invoke();
                }
            }
        }

        protected bool m_RuntimeMuted;

        public bool RuntimeMuted
        {
            get => m_RuntimeMuted;
            set
            {
                if (m_RuntimeMuted != value)
                {
                    m_RuntimeMuted = value;
                    OnMutedStateChanged?.Invoke();
                }
            }
        }

        [SerializeReference]
        protected List<Clip> m_Clips = new();

        public List<Clip> Clips => m_Clips;

        public Action OnUpdateMix;
        public Action OnMutedStateChanged;
        public Timeline Timeline { get; protected set; }
        public int MaxFrame { get; protected set; }

        public virtual void Init(Timeline timeline)
        {
            Timeline = timeline;
            MaxFrame = 0;
            foreach (Clip clip in m_Clips)
            {
                clip.Init(this);
                if (clip.EndFrame > MaxFrame)
                {
                    MaxFrame = clip.EndFrame;
                }
            }

            RuntimeMuted = false;
        }

        public virtual void Bind()
        {
            m_Clips.ForEach(c => c.Bind());
        }

        public virtual void UnBind()
        {
            m_Clips.ForEach(c => c.UnBind());
        }

        public virtual void ReBind()
        {
            UnBind();
            Bind();
        }

        public virtual void Evaluate(float deltaTime)
        {
            if (m_PersistentMuted || m_RuntimeMuted)
            {
                return;
            }

            m_Clips.ForEach(c => c.Evaluate(deltaTime));
        }

        public virtual void SetTime(float time)
        {
            // if (this.m_PersistentMuted || this.m_RuntimeMuted)
            // {
            //     return;
            // }
            //
            // m_Clips.ForEach(c => c.Evaluate(time));
        }

        public virtual void RuntimeMute(bool value)
        {
            if (this.PersistentMuted)
            {
                return;
            }

            if (value && !this.RuntimeMuted)
            {
                this.RuntimeMuted = true;
                this.UnBind();
            }
            else if (!value && this.RuntimeMuted)
            {
                RuntimeMuted = false;
                Bind();
                // SetTime(Timeline.Time);
            }
        }
    }

    [Serializable]
    public abstract partial class Clip
    {
        #region Frame

        public int StartFrame;
        public int EndFrame;
        public int OtherEaseInFrame;
        public int OtherEaseOutFrame;
        public int SelfEaseInFrame;
        public int SelfEaseOutFrame;
        public int ClipInFrame;

        public int EaseInFrame => OtherEaseInFrame == 0? SelfEaseInFrame : OtherEaseInFrame;
        public int EaseOutFrame => OtherEaseOutFrame == 0? SelfEaseOutFrame : OtherEaseOutFrame;
        public int Duration => this.EndFrame - this.StartFrame;

        #endregion

        #region Time

        public float StartTime { get; private set; }
        public float EndTime { get; private set; }
        public float OtherEaseInTime { get; private set; }
        public float OtherEaseOutTime { get; private set; }
        public float EaseInTime { get; private set; }
        public float EaseOutTime { get; private set; }
        public float ClipInTime { get; private set; }
        public float DurationTime { get; private set; }

        #endregion

        public bool CanSkip;

        [NonSerialized]
        public Track Track;

        public Timeline Timeline => this.Track.Timeline;
        public bool Active { get; protected set; }
        public float Time { get; protected set; }
        public float TargetTime { get; protected set; }
        public float OffsetTime => Time - StartTime + ClipInTime;

        public Action OnNameChanged;
        public Action OnInspectorRepaint;

        public virtual void Init(Track track)
        {
            Track = track;
            FrameToTime();
        }

        public virtual void Bind()
        {
            Active = false;
            Time = 0;
        }

        public virtual void UnBind()
        {
            if (Active)
            {
                OnDisable();
            }

            Active = false;
            Time = 0;
        }

        public virtual void Evaluate(float deltaTime)
        {
            TargetTime = Time + deltaTime;

            if (!Active && StartTime <= TargetTime && TargetTime <= EndTime)
            {
                Active = true;
                OnEnable();
            }
            else if (Active && (TargetTime < StartTime || EndTime < TargetTime))
            {
                Active = false;
                OnDisable();
            }

            if (!CanSkip)
            {
                if (!Active && Time < StartTime && EndTime < TargetTime)
                {
                    Active = true;
                    OnEnable();
                    Active = false;
                    OnDisable();
                }
                else if (!Active && TargetTime < StartTime && EndTime < Time)
                {
                    Active = true;
                    OnEnable();
                    Active = false;
                    OnDisable();
                }
            }

            Time = TargetTime;
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public void FrameToTime()
        {
            StartTime = StartFrame / (float)TimelineUtility.FrameRate;
            EndTime = EndFrame / (float)TimelineUtility.FrameRate;
            OtherEaseInTime = OtherEaseInFrame / (float)TimelineUtility.FrameRate;
            OtherEaseOutTime = OtherEaseOutFrame / (float)TimelineUtility.FrameRate;
            EaseInTime = EaseInFrame / (float)TimelineUtility.FrameRate;
            EaseOutTime = EaseOutFrame / (float)TimelineUtility.FrameRate;
            ClipInTime = ClipInFrame / (float)TimelineUtility.FrameRate;
            DurationTime = Duration / (float)TimelineUtility.FrameRate;
        }
    }

    public abstract partial class SignalClip: Clip
    {
        public override void Evaluate(float deltaTime)
        {
            TargetTime = Time + deltaTime;

            if (!Active && StartTime <= TargetTime)
            {
                Active = true;
                OnEnable();
            }
            else if (Active && TargetTime < StartTime)
            {
                Active = false;
                OnDisable();
            }

            Time = TargetTime;
        }
    }

#if UNITY_EDITOR
    public partial class Timeline
    {
        public float Scale = 1;
        public UnityEditor.SerializedObject SerializedTimeline;

        public void AddTrack(Type type)
        {
            Track track = Activator.CreateInstance(type) as Track;
            track.Name = type.Name.Replace("Track", string.Empty);
            m_Tracks.Add(track);
            Init();
        }

        public void RemoveTrack(Track track)
        {
            m_Tracks.Remove(track);
            Init();
        }

        public Clip AddClip(Track track, int frame)
        {
            //和其他clip重合
            foreach (Clip _clip in track.Clips)
            {
                if (_clip.Contains(frame))
                {
                    Debug.LogError("overlap with other clip!!!");
                    return null;
                }
            }

            Clip clip = track.AddClip(frame);
            Init();
            return clip;
        }

        public Clip AddClip(UnityEngine.Object referenceObject, Track track, int frame)
        {
            Clip clip = track.AddClip(referenceObject, frame);
            Init();
            return clip;
        }

        public void RemoveClip(Clip clip)
        {
            clip.Track.RemoveClip(clip);
            Init();
        }

        public void UpdateMix()
        {
            m_Tracks.ForEach(track => track.UpdateMix());
        }

        public void Resort()
        {
            OnValueChanged?.Invoke();
        }

        /// <summary>
        /// undo redo
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_name">Undo Redo 记录名 </param>
        public void ApplyModify(Action action, string _name)
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, $"Timeline: {_name}");
            SerializedTimeline.Update();
            action?.Invoke();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void UpdateSerializedTimeline()
        {
            SerializedTimeline = new UnityEditor.SerializedObject(this);
        }

        [UnityEditor.MenuItem("Assets/Create/ScriptableObject/Timeline/Timeline")]
        public static void CreateTimeline()
        {
            Timeline timeline = CreateInstance<Timeline>();
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/New Timeline.asset");
            UnityEditor.AssetDatabase.CreateAsset(timeline, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.Selection.activeObject = timeline;
        }
    }

    public abstract partial class Track
    {
        //AudioTrack ---> AudioClip
        public virtual Type ClipType => typeof (Clip);

        public virtual Clip AddClip(int frame)
        {
            Clip clip = Activator.CreateInstance(ClipType, this, frame) as Clip;
            m_Clips.Add(clip);
            return clip;
        }

        public virtual Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            return null;
        }

        public void RemoveClip(Clip clip)
        {
            m_Clips.Remove(clip);
        }

        public void UpdateMix()
        {
            Clips.ForEach(c =>
            {
                c.UpdateMix();
                c.FrameToTime();
            });
            OnUpdateMix?.Invoke();
        }

        public virtual bool DragValid()
        {
            return false;
        }

        public void RebindTimeline()
        {
            Timeline.RebindTrack(this);
        }
    }

    public abstract partial class Clip
    {
        [NonSerialized]
        public bool Invalid;

        public virtual string Name => GetType().Name;
        public virtual int Length => EndFrame - StartFrame;
        public virtual ClipCapabilities Capabilities => ClipCapabilities.None;

        public Clip()
        {
        }

        public Clip(Track track, int frame)
        {
            Track = track;
            StartFrame = frame;
            EndFrame = this.StartFrame + 3;
        }

        public void UpdateMix()
        {
            OtherEaseInFrame = 0;
            OtherEaseOutFrame = 0;

            if (Invalid)
            {
                return;
            }

            foreach (var clip in Track.Clips)
            {
                if (clip == this || clip.Invalid) continue;

                //包含
                if (clip.StartFrame < StartFrame && clip.EndFrame > EndFrame)
                {
                    return;
                }
                //被包含
                else if (clip.StartFrame > StartFrame && clip.EndFrame < EndFrame)
                {
                    return;
                }

                if (clip.StartFrame < StartFrame && clip.EndFrame > StartFrame)
                {
                    OtherEaseInFrame = clip.EndFrame - StartFrame;
                }

                if (clip.StartFrame > StartFrame && clip.StartFrame < EndFrame)
                {
                    OtherEaseOutFrame = EndFrame - clip.StartFrame;
                }

                if (clip.StartFrame == StartFrame)
                {
                    if (clip.EndFrame < EndFrame)
                    {
                        OtherEaseInFrame = clip.EndFrame - StartFrame;
                    }
                    else if (clip.EndFrame > EndFrame)
                    {
                        OtherEaseOutFrame = EndFrame - StartFrame;
                    }
                }

                SelfEaseInFrame = Mathf.Min(SelfEaseInFrame, Duration - OtherEaseOutFrame);
                SelfEaseOutFrame = Mathf.Min(SelfEaseOutFrame, Duration - OtherEaseInFrame);
            }
        }

        public bool Contains(float halfFrame)
        {
            return StartFrame < halfFrame && halfFrame < EndFrame;
        }

        /// <summary>
        /// 和其他clip重叠
        /// </summary>
        public bool Overlap(Clip clip)
        {
            for (int i = clip.StartFrame; i <= clip.EndFrame; i++)
            {
                if (Contains(i)) return true;
            }

            return false;
        }

        public string StartTimeText()
        {
            return $"StartTime: {StartTime:0.00}S / StartFrame: {StartFrame}";
        }

        public string EndTimeText()
        {
            return $"EndTime: {EndTime:0.00}S / StartFrame: {EndFrame}";
        }

        public string DurationText()
        {
            return $"Duration: {DurationTime:0.00}S / {Duration}";
        }

        public virtual void RebindTimeline()
        {
            Track.RebindTimeline();
        }

        public virtual void RepaintInspector()
        {
            OnInspectorRepaint?.Invoke();
        }

        public virtual bool IsResizeable()
        {
            return (Capabilities & ClipCapabilities.Resizeable) == ClipCapabilities.Resizeable;
        }

        public virtual bool IsMixable()
        {
            return (Capabilities & ClipCapabilities.Mixable) == ClipCapabilities.Mixable;
        }

        public virtual bool IsClipInable()
        {
            return (Capabilities & ClipCapabilities.ClipInable) == ClipCapabilities.ClipInable;
        }
    }

    public abstract partial class SignalClip
    {
        protected SignalClip(Track track, int frame): base(track, frame)
        {
            EndFrame = StartFrame + 1;
        }
    }
#endif
}