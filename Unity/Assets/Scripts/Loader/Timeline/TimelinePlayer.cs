using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    public class TimelinePlayer: MonoBehaviour
    {
        public RuntimeAnimatorController Controller;
        public bool ApplyRootMotion;
        public bool m_IsPlaying;

        public bool IsPlaying
        {
            get => m_IsPlaying;
            set
            {
                if (m_IsPlaying == value)
                {
                    return;
                }

                this.m_IsPlaying = value;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (m_IsPlaying)
                    {
                        EditorApplication.update += EditorUpdate;
                    }
                    else
                    {
                        EditorApplication.update -= EditorUpdate;
                    }
                }
#endif
            }
        }

        public double m_PlaySpeed;

        public double PlaySpeed
        {
            get => Math.Round(Math.Max(0.001f, this.m_PlaySpeed), 2);
            set => this.m_PlaySpeed = value;
        }

        public bool IsValid => this.PlayableGraph.IsValid();
        public Animator Animator { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        public AnimatorControllerPlayable CtrlPlayable { get; private set; }
        public AudioMixerPlayable AudioRootPlayable { get; private set; }
        public List<Timeline> RunningTimelines { get; private set; }
        public List<RuntimeTrackEaseOut> RuntimeTrackEaseOuts { get; private set; }
        public float AddtionalDelta { get; set; }
        public event Action OnEvaluated;

        protected virtual void OnEnable()
        {
            Init();
            IsPlaying = true;
        }

        protected void OnDisable()
        {
            Dispose();
        }

        protected void FixedUpdate()
        {
            if (IsPlaying)
            {
                Evaluate(Time.deltaTime * (float)PlaySpeed);
                if (AddtionalDelta != 0)
                {
                    Evaluate(AddtionalDelta);
                    AddtionalDelta = 0;
                }
            }
        }

        protected virtual void Update()
        {
        }

#if UNITY_EDITOR
        public void EditorUpdate()
        {
        }
#endif

        public virtual void Init()
        {
            PlayableGraph = PlayableGraph.Create("Blazblue.Timeline.PlayableGraph");
            //混合
            AnimationRootPlayable = AnimationLayerMixerPlayable.Create(this.PlayableGraph);
            AudioRootPlayable = AudioMixerPlayable.Create(PlayableGraph);    
            
            Animator = GetComponent<Animator>();
            var playableOutput = AnimationPlayableOutput.Create(PlayableGraph, "Animation", Animator);
            playableOutput.SetSourcePlayable(AnimationRootPlayable);

            AudioSource = GetComponent<AudioSource>();
            var audioOutput = AudioPlayableOutput.Create(PlayableGraph, "Audio", GetComponent<AudioSource>());
            audioOutput.SetSourcePlayable(AudioRootPlayable);
            audioOutput.SetEvaluateOnSeek(true);

            CtrlPlayable = AnimatorControllerPlayable.Create(PlayableGraph, Controller);
            AnimationRootPlayable.AddInput(CtrlPlayable, 0, 1);

            RunningTimelines = new List<Timeline>();
            RuntimeTrackEaseOuts = new List<RuntimeTrackEaseOut>();

            IsPlaying = false;
            PlaySpeed = 1;
        }

        public virtual void Dispose()
        {
            if (this.IsValid)
            {
                for (int i = RunningTimelines.Count - 1; i >= 0; i--)
                {
                    RemoveTimeline(RunningTimelines[i]);
                }
                PlayableGraph.Destroy();
            }
            RunningTimelines = null;
            IsPlaying = false;
            PlaySpeed = 1;
        }

        public virtual void Evaluate(float deltaTime)
        {
            for (int i = RunningTimelines.Count - 1; i >= 0; i--)
            {
                Timeline runningTimelines = RunningTimelines[i];
                runningTimelines.Evaluate(deltaTime);
            }

            PlayableGraph.Evaluate(deltaTime);
        }

        protected virtual void OnRootMotion()
        {
            if (ApplyRootMotion)
            {
                transform.position += Animator.deltaPosition;
            }
        }

        #region Animator

        public virtual void SetFloat(string _name, float value)
        {
            CtrlPlayable.SetFloat(_name, value);
        }

        public virtual float GetFloat(string _name)
        {
            return CtrlPlayable.GetFloat(_name);
        }

        public virtual void SetBool(string _name, bool value)
        {
            CtrlPlayable.SetBool(_name, value);
        }

        public virtual bool GetBool(string _name)
        {
            return CtrlPlayable.GetBool(_name);
        }

        public virtual void SetTrigger(string _name)
        {
            CtrlPlayable.SetTrigger(_name);
        }

        public virtual void SetStateTime(string _name, float time, int layer)
        {
            CtrlPlayable.CrossFade(name, 0, layer, time);
        }

        #endregion

        public virtual void AddTimeline(Timeline timeline)
        {
            // timeline.Init();
            timeline.Bind(this);
            RunningTimelines.Add(timeline);
            Evaluate(0);
        }

        public virtual void RemoveTimeline(Timeline timeline)
        {
            timeline.UnBind();
            RunningTimelines.Remove(timeline);
            if (RunningTimelines.Count == 0)
            {
                AnimationRootPlayable.SetInputCount(1);
            }
        }

        public virtual void AddAnimationEaseOut(AnimationTrack animationTrack)
        {
            RuntimeTrackEaseOut runtimeTrackEaseOut = new RuntimeTrackEaseOut(AnimationRootPlayable, animationTrack);
            RuntimeTrackEaseOuts.Add(runtimeTrackEaseOut);
        }
    }

    public class RuntimeTrackEaseOut
    {
        public Playable Root;
        public Playable Track;
        public int Index;
        public float EaseOutTime;
        public float Timer;
        public float OriginalWeight;

        public RuntimeTrackEaseOut(Playable root, AnimationTrack animationTrack)
        {
            Root = root;
            Track = animationTrack.TrackPlayable.Handle;
            if (!animationTrack.PlayWhenEaseOut)
            {
                Track.Pause();
            }

            Index = animationTrack.PlayableIndex;
            EaseOutTime = animationTrack.EaseOutTime;

            OriginalWeight = Root.GetInputWeight(Index);
            Timer = 0;
        }

        public void Evaluate(float deltaTime)
        {
            Timer += deltaTime;
            Root.SetInputWeight(Index, Mathf.Lerp(OriginalWeight, 0, Timer / EaseOutTime));
        }
    }
}