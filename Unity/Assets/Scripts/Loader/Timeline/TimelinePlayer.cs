using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    #region Event

    public struct BehaviorControllerReloadCallback
    {
        public long instanceId;
    }

    public struct PreviewReloadCallback
    {
        public long instanceId;
        public BehaviorClip Clip;
    }

    public struct EditTimelineCallback
    {
        public long instanceId;
    }

    #endregion

    public sealed class TimelinePlayer: SerializedMonoBehaviour
    {
        [HideInInspector]
        public long instanceId; // DialogueComponent
        
        public bool ApplyRootMotion;

        public bool IsValid => PlayableGraph.IsValid();
        private Animator Animator { get; set; }
        public AudioSource AudioSource { get; private set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        private AudioMixerPlayable AudioRootPlayable { get; set; }

        [ShowIf("HasNotBindUnit")]
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

        // ReSharper disable once Unity.RedundantEventFunction
        private void OnAnimatorMove()
        {
            //禁用AnimationClip对transform的修改
        }

#if UNITY_EDITOR
        [Button("行为编辑器"), ShowIf("HasNotBindUnit")]
        public void OpenController()
        {
            BehaviorControllerEditor.OpenWindow(this);
        }

        [Button("技能编辑器"), ShowIf("HasNotBindUnit")]
        public void OpenWindow()
        {
            var timelineSet = BBPlayable.GetTimelines();
            if (timelineSet.Count <= 0)
            {
                Debug.LogWarning("PlayableGraph need at least one timeline");
                return;
            }

            foreach (var timeline in timelineSet)
            {
                OpenWindow(timeline);
                return;
            }
        }

        public void OpenWindow(BBTimeline timeline)
        {
            ClearTimelineGenerate();
            TimelineEditorWindow.OpenWindow(this, timeline);
        }

        [Button("清除运行时组件"), ShowIf("HasNotBindUnit")]
        public void ClearTimelineGenerate()
        {
            var goSet = new HashSet<GameObject>();
            foreach (var component in GetComponentsInChildren<Component>())
            {
                if (component.GetComponent<TimelineGenerate>() != null)
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

        public void Init(int order)
        {
            Init(BBPlayable.GetByOrder(order));
        }

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

        public BBTimeline GetByOrder(int order)
        {
            return CurrentTimeline;
        }

        /// <summary>
        /// 运行时 逻辑层传回组件instanceId给loader层回调事件
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasBindUnit
        {
            get
            {
                return instanceId != 0;
            }
        }

        public bool HasNotBindUnit
        {
            get => !HasBindUnit;
        }
    }
}