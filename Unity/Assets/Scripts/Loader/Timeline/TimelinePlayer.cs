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
        [DictionaryDrawerSettings(KeyLabel = "BehaviorOrder", ValueLabel = "Timeline")]
        public Dictionary<int, BBTimeline> Timelines = new();

        [HideInInspector]
        public BBTimeline CurrentTimeline;

        [HideInInspector]
        public RuntimePlayable RuntimeimePlayable;

        public void OnDisable()
        {
            Dispose();
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("技能编辑器")]
        public void OpenWindow()
        {
            //默认字典第一个元素为入口
            foreach (var pair in Timelines)
            {
                OpenWindow(pair.Value);
                break;
            }
        }

        public void OpenWindow(BBTimeline timeline)
        {
            ClearTimelineGenerate();
            TimelineEditorWindow.OpenWindow(this, timeline);
        }

        [Sirenix.OdinInspector.Button("清除运行时组件")]
        public void ClearTimelineGenerate()
        {
            var goSet = new HashSet<GameObject>();
            foreach (var component in GetComponentsInChildren<Component>())
            {
                if (component.GetAttribute<TimelineGenerateAttribute>() != null)
                {
                    goSet.Add(component.gameObject);
                }
            }

            foreach (GameObject go in goSet)
            {
                DestroyImmediate(go);
            }
        }

        public void EditorUpdate()
        {
        }
#endif

        public void Init(BBTimeline _timeline)
        {
            #region PlayableGraph

            PlayableGraph = PlayableGraph.Create(_timeline.timelineName);
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

            CurrentTimeline = _timeline;
            RuntimeimePlayable = RuntimePlayable.Create(CurrentTimeline, this);

            #endregion
        }

        public void Dispose()
        {
            if (PlayableGraph.IsValid()) PlayableGraph.Destroy();
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