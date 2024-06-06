using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using Timeline.Editor;
#endif
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    [Serializable]
    [BBTrack("Animation")]
#if UNITY_EDITOR
    [Color(127, 253, 228)]
    [IconGuid("46d1be470ea7f7945b52ec8511f9a419")]
#endif
    public class BBAnimationTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeAnimationTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBAnimationClip);
        public override Type ClipViewType => typeof (AnimationClipView);
#endif
    }

#if UNITY_EDITOR
    [Color(127, 253, 228)]
#endif
    public class BBAnimationClip: BBClip
    {
        public UnityEngine.AnimationClip animationClip;

        //rootmotion data
        public Dictionary<string, RootMotionData> rootMotionDict = new();

        public Vector3 DeltaPosition(int targetFrame)
        {
            float x = GetRootMotionData("m_LocalPosition.x", targetFrame);
            float y = GetRootMotionData("m_LocalPosition.y", targetFrame);
            float z = GetRootMotionData("m_LocalPosition.z", targetFrame);
            return new Vector3(x, y, z);
        }

        public Vector3 DeltaRotation(int targetFrame)
        {
            float x = GetRootMotionData("localEulerAnglesRaw.x", targetFrame);
            float y = GetRootMotionData("localEulerAnglesRaw.y", targetFrame);
            float z = GetRootMotionData("localEulerAnglesRaw.z", targetFrame);
            return new Vector3(x, y, z);
        }

        private float GetRootMotionData(string key, int targetFrame)
        {
            if (!rootMotionDict.TryGetValue(key, out RootMotionData rootMotionData)) return 0;
            rootMotionData.datas.TryGetValue(targetFrame, out float value);
            return value;
        }

        public BBAnimationClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (AnimationClipInspectorData);
#endif
    }

    [Serializable]
    public class RootMotionData
    {
        public string propertyName;

        public Dictionary<int, float> datas = new();
    }

    #region Editor

    [Serializable]
    public class AnimationClipInspectorData: ShowInspectorData
    {
        private BBAnimationClip Clip;
        private TimelineFieldView FieldView;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [LabelText("Clip: ")]
        public UnityEngine.AnimationClip AnimationClip;

        [LabelText("AnimationLength: ")]
        [Sirenix.OdinInspector.ShowInInspector]
        public int animationLength => AnimationClip == null? 0 : (int)(AnimationClip.length * TimelineUtility.FrameRate);

        [Sirenix.OdinInspector.Button("Rebind")]
        public void Rebind()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                Clip.animationClip = AnimationClip;
                Clip.rootMotionDict.Clear();

                foreach (var binding in AnimationUtility.GetCurveBindings(Clip.animationClip))
                {
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(Clip.animationClip, binding);
                    string propertyName = binding.propertyName;

                    RootMotionData rootMotionData = new();
                    rootMotionData.propertyName = propertyName;

                    float currentValue = 0f;
                    for (int i = 0; i < curve.keys.Length; i++)
                    {
                        Keyframe key = curve.keys[i];
                        int targetFrame = Mathf.RoundToInt(key.time * TimelineUtility.FrameRate);

                        float deltaValue = key.value - currentValue;
                        currentValue = key.value;

                        rootMotionData.datas.Add(targetFrame, deltaValue);
                    }

                    Clip.rootMotionDict.Add(propertyName, rootMotionData);
                }
            }, "rebind animationClip");
        }

        public AnimationClipInspectorData(object target): base(target)
        {
            Clip = target as BBAnimationClip;
            AnimationClip = Clip.animationClip;
        }

        private void UpdateSprite()
        {
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            EditorApplication.update += UpdateSprite;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
            EditorApplication.update -= UpdateSprite;
        }
    }

    #endregion

    #region Runtime

    public class RuntimeAnimationTrack: RuntimeTrack
    {
        public BBAnimationTrack AnimationTrack => Track as BBAnimationTrack;
        private BBTimelineAnimationTrackPlayable TrackPlayable;
        private AnimationMixerPlayable MixerPlayable => TrackPlayable.MixerPlayable;
        private readonly List<BBTimelineAnimationClipPlayable> ClipPlayables = new();

        public override void Bind()
        {
            TrackPlayable = BBTimelineAnimationTrackPlayable.Create(RuntimePlayable, this, RuntimePlayable.AnimationRootPlayable);
            PlayableIndex = RuntimePlayable.AnimationRootPlayable.GetInputCount() - 1;
            RuntimePlayable.AnimationRootPlayable.SetInputWeight(PlayableIndex, 1);

            ClipPlayables.Clear();
            for (int i = 0; i < AnimationTrack.Clips.Count; i++)
            {
                BBTimelineAnimationClipPlayable clipPlayable =
                        BBTimelineAnimationClipPlayable.Create(RuntimePlayable, AnimationTrack.Clips[i] as BBAnimationClip, MixerPlayable, i);
                ClipPlayables.Add(clipPlayable);
            }
        }

        public override void UnBind()
        {
            for (int i = 0; i < ClipPlayables.Count; i++)
            {
                //Destroy clipPlayable
                BBTimelineAnimationClipPlayable clipPlayable = ClipPlayables[i];
                MixerPlayable.DisconnectInput(i);
                clipPlayable.Handle.Destroy();
            }

            // Destroy trackPlayable
            RuntimePlayable.AnimationRootPlayable.DisconnectInput(PlayableIndex);
            TrackPlayable.Handle.Destroy();
        }

        public override void SetTime(int targetFrame)
        {
            for (int i = 0; i < ClipPlayables.Count; i++)
            {
                BBTimelineAnimationClipPlayable clipPlayable = ClipPlayables[i];
                clipPlayable.SetInputWeight((clipPlayable.Clip.InMiddle(targetFrame))? 1 : 0);
                clipPlayable.SetTime(targetFrame);
            }
        }

        public override void RuntimMute(bool value)
        {
        }

        public RuntimeAnimationTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }
    }

    public class BBTimelineAnimationTrackPlayable: PlayableBehaviour
    {
        private RuntimePlayable runtimePlayable;
        private RuntimeAnimationTrack runtimeTrack;
        private BBAnimationTrack Track { get; set; }
        private Playable Output { get; set; }
        public Playable Handle { get; private set; }
        public AnimationMixerPlayable MixerPlayable { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public static BBTimelineAnimationTrackPlayable Create(RuntimePlayable runtimePlayable, RuntimeAnimationTrack runtimeAnimationTrack,
        Playable output)
        {
            var handle = ScriptPlayable<BBTimelineAnimationTrackPlayable>.Create(runtimePlayable.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.runtimeTrack = runtimeAnimationTrack;
            trackPlayable.Track = runtimeAnimationTrack.AnimationTrack;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AnimationMixerPlayable.Create(runtimePlayable.PlayableGraph, runtimeAnimationTrack.ClipCount);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0, 0);
            return trackPlayable;
        }
    }

    public class BBTimelineAnimationClipPlayable: PlayableBehaviour
    {
        public BBClip Clip { get; private set; }
        private int Index { get; set; }
        private Playable Output { get; set; }
        public Playable Handle { get; private set; }
        private AnimationClipPlayable ClipPlayable { get; set; }
        private RuntimePlayable runtimePlayable;
        private int currentFrame = -1;
        
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public void SetInputWeight(float weight)
        {
            Output.SetInputWeight(Index, weight);
        }

        public float GetInputWeight()
        {
            return Output.GetInputWeight(Index);
        }

        public void SetTime(int targetFrame)
        {
            if (targetFrame == currentFrame) return;
            currentFrame = targetFrame;
            
            int clipInFrame = targetFrame - Clip.StartFrame;
            ClipPlayable.SetTime((float)clipInFrame / TimelineUtility.FrameRate);
            PrepareFrame(default, default);
            
            //Apply rootMotion
            // if (!runtimePlayable.TimelinePlayer.ApplyRootMotion)
            // {
            //     return;
            // }
            //
            // BBAnimationClip animationClip = Clip as BBAnimationClip;
            // Transform trans = runtimePlayable.TimelinePlayer.transform;
            //
            // trans.localPosition += animationClip.DeltaPosition(clipInFrame);
            //
            // Debug.LogWarning(trans.localPosition);
        }

        public static BBTimelineAnimationClipPlayable Create(RuntimePlayable runtimePlayable, BBAnimationClip clip, Playable output, int index)
        {
            var handle = ScriptPlayable<BBTimelineAnimationClipPlayable>.Create(runtimePlayable.PlayableGraph);
            var clipPlayable = handle.GetBehaviour();
            clipPlayable.Clip = clip;
            clipPlayable.Handle = handle;
            clipPlayable.ClipPlayable = AnimationClipPlayable.Create(runtimePlayable.PlayableGraph, clip.animationClip);
            handle.AddInput(clipPlayable.ClipPlayable, 0, 1);

            clipPlayable.Output = output;
            clipPlayable.Index = index;
            clipPlayable.runtimePlayable = runtimePlayable;
            output.ConnectInput(index, handle, 0, 0);

            return clipPlayable;
        }
    }

    #endregion
}