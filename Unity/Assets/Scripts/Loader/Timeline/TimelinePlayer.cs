using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    public sealed class TimelinePlayer: SerializedMonoBehaviour
    {
        public bool ApplyRootMotion;

        public bool IsValid => PlayableGraph.IsValid();
        private Animator Animator { get; set; }
        public AudioSource AudioSource { get; private set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        public AudioMixerPlayable AudioRootPlayable { get; private set; }

        public BBPlayableGraph BBPlayable;

        [HideInInspector]
        public BBTimeline CurrentTimeline;

        [HideInInspector]
        public RuntimePlayable RuntimeimePlayable;

        public void OnDisable()
        {
            Dispose();
        }

        private Vector2 initPos;

        public void Start()
        {
            initPos = transform.position;
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("技能编辑器")]
        public void OpenWindow()
        {
            //默认字典第一个元素为入口
            foreach (var pair in BBPlayable.Timelines)
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

        [Sirenix.OdinInspector.Button("初始位置")]
        public void ResetPosition()
        {
            transform.position = initPos;
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

        // private void OnRootMotion()
        // {
        //     if (ApplyRootMotion)
        //     {
        //         transform.position += Animator.deltaPosition;
        //     }
        // }
        public void OnRootMotion()
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

        public BBTimeline GetByOrder(int order)
        {
            if (BBPlayable == null) return null;
            if (!BBPlayable.Timelines.TryGetValue(order, out BBTimeline timeline))
            {
                Debug.LogError($"not exist timeline, order: {order}");
                return null;
            }

            return timeline;
        }
    }
}