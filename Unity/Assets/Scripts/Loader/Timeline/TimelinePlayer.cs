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
        public bool ApplyRootMotion;

        private bool m_IsPlaying;

        public bool IsPlaying
        {
            get => m_IsPlaying;
            set
            {
                if (m_IsPlaying == value)
                {
                    return;
                }

                m_IsPlaying = value;
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
            get => Math.Round(Math.Max(0.001f, m_PlaySpeed), 2);
            set => m_PlaySpeed = value;
        }

        public bool IsValid => PlayableGraph.IsValid();
        private Animator Animator { get; set; }
        public AudioSource AudioSource { get; private set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        public AudioMixerPlayable AudioRootPlayable { get; private set; }

        public Timeline RunningTimeline;

        public float AddtionalDelta { get; set; }
        public event Action OnEvaluated;

        protected virtual void OnEnable()
        {
            // Init();
            // IsPlaying = true;
        }

        protected void OnDisable()
        {
            // Dispose();
        }

        // protected void FixedUpdate()
        // {
        //     if (IsPlaying)
        //     {
        //         Evaluate(Time.deltaTime * (float)PlaySpeed);
        //         if (AddtionalDelta != 0)
        //         {
        //             Evaluate(AddtionalDelta);
        //             AddtionalDelta = 0;
        //         }
        //     }
        // }

#if UNITY_EDITOR
        public void EditorUpdate()
        {
        }
#endif

        public virtual void Init()
        {
            PlayableGraph = PlayableGraph.Create("BBScript");
            //混合
            AnimationRootPlayable = AnimationLayerMixerPlayable.Create(PlayableGraph);
            AudioRootPlayable = AudioMixerPlayable.Create(PlayableGraph);

            Animator = GetComponent<Animator>();
            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(PlayableGraph, "Animation", Animator);
            playableOutput.SetSourcePlayable(AnimationRootPlayable);

            AudioSource = GetComponent<AudioSource>();
            AudioPlayableOutput audioOutput = AudioPlayableOutput.Create(PlayableGraph, "Audio", GetComponent<AudioSource>());
            audioOutput.SetSourcePlayable(AudioRootPlayable);
            audioOutput.SetEvaluateOnSeek(true);

            IsPlaying = false;
            PlaySpeed = 1;
        }



        public virtual void Dispose()
        {
            // if (IsValid)
            // {
            //     for (int i = RunningTimelines.Count - 1; i >= 0; i--)
            //     {
            //         // RemoveTimeline(RunningTimelines[i]);
            //     }
            //     PlayableGraph.Destroy();
            // }
            // RunningTimelines = null;
            // IsPlaying = false;
            // PlaySpeed = 1;
        }

        public virtual void Evaluate(float deltaTime)
        {
            // for (int i = RunningTimelines.Count - 1; i >= 0; i--)
            // {
            //     // Timeline runningTimelines = RunningTimelines[i];
            //     // runningTimelines.Evaluate(deltaTime);
            // }
            // PlayableGraph.Evaluate(deltaTime);
            //
            // OnRootMotion();
            //
            // OnEvaluated?.Invoke();
        }

        protected virtual void OnRootMotion()
        {
            if (ApplyRootMotion)
            {
                transform.position += Animator.deltaPosition;
            }
        }

        // public virtual void AddTimeline(Timeline timeline)
        // {
        //     timeline.UnBind();
        //     timeline.Bind(this);
        //
        //     // RunningTimelines.Remove(timeline);
        //     // RunningTimelines.Add(timeline);
        //     Evaluate(0);
        // }

        public virtual void BindTimeline(Timeline timeline)
        {
            timeline.UnBind();
            timeline.Bind(this);
            RunningTimeline = timeline;
            Evaluate(0);
        }

        public virtual void RemoveTimeline(Timeline timeline)
        {
            timeline.UnBind();
            // RunningTimelines.Remove(timeline);
            // if (RunningTimelines.Count == 0)
            // {
            //     AnimationRootPlayable.SetInputCount(1);
            // }
        }

        // public virtual void AddAnimationEaseOut(AnimationTrack animationTrack)
        // {
        //     RuntimeTrackEaseOut runtimeTrackEaseOut = new(AnimationRootPlayable, animationTrack);
        //     RuntimeTrackEaseOuts.Add(runtimeTrackEaseOut);
        // }
    }

    public class RuntimeTrackEaseOut
    {
        private readonly Playable Root;
        public Playable Track;
        public int Index;
        public float EaseOutTime;
        public float Timer;
        public float OriginalWeight;

        public RuntimeTrackEaseOut(Playable root, AnimationTrack animationTrack)
        {
            // Root = root;
            // Track = animationTrack.TrackPlayable.Handle;
            // if (!animationTrack.PlayWhenEaseOut)
            // {
            //     Track.Pause();
            // }
            //
            // Index = animationTrack.PlayableIndex;
            // EaseOutTime = animationTrack.EaseOutTime;
            //
            // OriginalWeight = Root.GetInputWeight(Index);
            // Timer = 0;
        }

        public void Evaluate(float deltaTime)
        {
            Timer += deltaTime;
            Root.SetInputWeight(Index, Mathf.Lerp(OriginalWeight, 0, Timer / EaseOutTime));
        }
    }
}