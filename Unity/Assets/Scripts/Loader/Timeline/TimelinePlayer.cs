using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace ET
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
        
        public float AddtionalDelta { get; set; }
        public event Action OnEvaluated;

        protected virtual void OnEnable()
        {
            this.Init();
            this.IsPlaying = true;
        }

        protected void OnDisable()
        {
            this.Dispose();
        }

        protected virtual void Update()
        {
            
        }

        protected void FixedUpdate()
        {
            if (this.IsPlaying)
            {
                Evaluate(Time.deltaTime * (float)this.PlaySpeed);
            }
        }

#if UNITY_EDITOR
        public void EditorUpdate()
        {
        }
#endif

        public virtual void Init()
        {
            this.PlayableGraph = PlayableGraph.Create("Blazblue.Timeline.PlayableGraph");
            this.AnimationRootPlayable = AnimationLayerMixerPlayable.Create(this.PlayableGraph);

            Animator = this.GetComponent<Animator>();
            var playableOutput = AnimationPlayableOutput.Create(this.PlayableGraph, "Animation", this.Animator);
            playableOutput.SetSourcePlayable(this.AnimationRootPlayable);

            AudioSource = this.GetComponent<AudioSource>();
            var audioOutput = AudioPlayableOutput.Create(this.PlayableGraph, "Audio", this.GetComponent<AudioSource>());
            audioOutput.SetSourcePlayable(this.AudioRootPlayable);
            audioOutput.SetEvaluateOnSeek(true);

            this.CtrlPlayable = AnimatorControllerPlayable.Create(PlayableGraph, Controller);
            this.AnimationRootPlayable.AddInput(CtrlPlayable, 0, 1);
        }

        public virtual void Dispose()
        {
            if (this.IsValid)
            {
                for (int i = this.RunningTimelines.Count - 1; i >= 0; i--)
                {
                    RemoveTimeline(this.RunningTimelines[i]);
                }

                this.PlayableGraph.Destroy();
            }

            this.RunningTimelines = null;
            this.IsPlaying = false;
            this.PlaySpeed = 1;
        }

        public virtual void Evaluate(float deltaTime)
        {
            for (int i = this.RunningTimelines.Count - 1; i >= 0; i--)
            {
                Timeline runningTimelines = this.RunningTimelines[i];
                runningTimelines.Evaluate(deltaTime);
            }
            this.PlayableGraph.Evaluate(deltaTime);
        }

        protected virtual void OnRootMotion()
        {
            if (this.ApplyRootMotion)
            {
                this.transform.position += this.Animator.deltaPosition;
            }
        }

        public virtual void RemoveTimeline(Timeline timeline)
        {
            
        }

        public virtual void AddTimeline(Timeline timeline)
        {
            timeline.Init();
            
        }
    }
}