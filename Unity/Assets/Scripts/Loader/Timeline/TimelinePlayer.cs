using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    public sealed class TimelinePlayer: SerializedMonoBehaviour
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

        public bool IsValid => PlayableGraph.IsValid();
        private Animator Animator { get; set; }
        public AudioSource AudioSource { get; private set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        public AudioMixerPlayable AudioRootPlayable { get; private set; }

        [HideReferenceObjectPicker]
        [DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "Timeline")]
        public Dictionary<string, BBTimeline> Timelines = new();

        public BBRuntimePlayable RuntimeimePlayable;

#if UNITY_EDITOR
        [ButtonGroup("技能编辑器")]
        public void OpenWindow()
        {
            TimelineEditorWindow.OpenWindow(this);
        }

        public void EditorUpdate()
        {
        }
#endif

        public void Init()
        {
            BBTimeline timeline = Timelines["Test1"];

            #region PlayableGraph

            PlayableGraph = PlayableGraph.Create("Test1");
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

            #endregion

            IsPlaying = false;

            #region RuntimeTimeline

            RuntimeimePlayable = BBRuntimePlayable.Create(timeline, this);

            #endregion
        }

        public void Dispose()
        {
        }

        public void Evaluate(float deltaTime)
        {
        }

        private void OnRootMotion()
        {
            if (ApplyRootMotion)
            {
                transform.position += Animator.deltaPosition;
            }
        }

        public void BindTimeline(Timeline timeline)
        {
        }

        public void RemoveTimeline(Timeline timeline)
        {
        }
    }
}