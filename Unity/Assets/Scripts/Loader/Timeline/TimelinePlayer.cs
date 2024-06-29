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
        [HideInInspector]
        public long instanceId; // DialogueComponent

        [OnValueChanged("ResetPos")]
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

        [HideInInspector]
        //根运动的初始位置，跟animationCurve中的差值即为这一帧的移动距离
        public Vector3 initPos;

        public void OnEnable()
        {
            initPos = transform.localPosition;
        }

        private void OnAnimatorMove()
        {
            //禁用AnimationClip对transform的修改
        }

        private void ResetPos()
        {
            transform.localPosition = initPos;
        }

        
        
#if UNITY_EDITOR
        [Button("行为编辑器")]
        public void OpenController()
        {
            BehaviorControllerEditor.OpenWindow(this);
        }

        [Button("技能编辑器")]
        public void OpenWindow()
        {
            //默认字典第一个元素为入口
            // foreach (var behaviorClip in BBPlayable.BehaviorClips)
            // {
            //     if (behaviorClip.Timeline == null) continue;
            //     OpenWindow(behaviorClip.Timeline);
            //     return;
            // }

            Debug.LogWarning("PlayableGraph need at least one timeline");
        }

        public void OpenWindow(BBTimeline timeline)
        {
            ClearTimelineGenerate();
            TimelineEditorWindow.OpenWindow(this, timeline);
        }

        [Button("清除运行时组件")]
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

            // if (!ApplyRootMotion)
            // {
            //     ResetPos();
            // }

            #endregion
        }

        public void Dispose()
        {
            if (PlayableGraph.IsValid()) PlayableGraph.Destroy();
            ResetPos();
        }
        
        public BBTimeline GetByOrder(int order)
        {
            // if (BBPlayable == null) return null;
            // if (!BBPlayable.Timelines.TryGetValue(order, out BBTimeline timeline))
            // {
            //     Debug.LogError($"not exist timeline, order: {order}");
            //     return null;
            // }

            // return timeline;
            return CurrentTimeline;
        }
    }
}