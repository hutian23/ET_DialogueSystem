using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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
        [OdinSerialize, NonSerialized]
        public Dictionary<string, AnimationCurve> rootMotionDict = new();

        public Vector3 CurrentPosition(int targetFrame)
        {
            float x = GetRootMotionData("m_LocalPosition_x", targetFrame);
            float y = GetRootMotionData("m_LocalPosition_y", targetFrame);
            float z = GetRootMotionData("m_LocalPosition_z", targetFrame);
            return new Vector3(x, y, z);
        }

        public Vector3 CurrentRotation(int targetFrame)
        {
            float x = GetRootMotionData("localEulerAnglesRaw_x", targetFrame);
            float y = GetRootMotionData("localEulerAnglesRaw_y", targetFrame);
            float z = GetRootMotionData("localEulerAnglesRaw_z", targetFrame);
            return new Vector3(x, y, z);
        }

        private float GetRootMotionData(string key, int targetFrame)
        {
            if (!rootMotionDict.TryGetValue(key, out AnimationCurve curve)) return 0f;
            float initPos = curve.Evaluate(0);
            return curve.Evaluate((float)targetFrame / TimelineUtility.FrameRate) - initPos;
        }

        public BBAnimationClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (AnimationClipInspectorData);
#endif
    }

    #region Editor

    [Serializable]
    public class AnimationClipInspectorData: ShowInspectorData
    {
        private BBAnimationClip Clip;
        private TimelineFieldView FieldView;

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
                    string propertyName = binding.propertyName.Replace(".", "_");
                    AnimationCurve cloneCurve = MongoHelper.Clone(curve);
                    Clip.rootMotionDict.TryAdd(propertyName, cloneCurve);
                }
            }, "rebind animationClip");
        }

        public AnimationClipInspectorData(object target): base(target)
        {
            Clip = target as BBAnimationClip;
            AnimationClip = Clip.animationClip;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
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
                BBTimelineAnimationClipPlayable clipPlayable = BBTimelineAnimationClipPlayable.Create(RuntimePlayable, AnimationTrack.Clips[i] as BBAnimationClip, MixerPlayable, i);
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
                clipPlayable.SetInputWeight(clipPlayable.Clip.InMiddle(targetFrame)? 1 : 0);
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
        private BBAnimationTrack Track { get; set; }
        private Playable Output { get; set; }
        public Playable Handle { get; private set; }
        public AnimationMixerPlayable MixerPlayable { get; private set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public static BBTimelineAnimationTrackPlayable Create(RuntimePlayable runtimePlayable, RuntimeAnimationTrack runtimeAnimationTrack,
        Playable output)
        {
            var handle = ScriptPlayable<BBTimelineAnimationTrackPlayable>.Create(runtimePlayable.PlayableGraph);
            var trackPlayable = handle.GetBehaviour();
            trackPlayable.Track = runtimeAnimationTrack.AnimationTrack;
            trackPlayable.Handle = handle;
            trackPlayable.MixerPlayable = AnimationMixerPlayable.Create(runtimePlayable.PlayableGraph, runtimeAnimationTrack.ClipCount);
            handle.AddInput(trackPlayable.MixerPlayable, 0, 1);

            trackPlayable.Output = output;
            output.AddInput(handle, 0);
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

        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public void SetInputWeight(float weight)
        {
            Output.SetInputWeight(Index, weight);
        }

        private float GetInputWeight()
        {
            return Output.GetInputWeight(Index);
        }

        public void SetTime(int targetFrame)
        {
            //Evaluate Clip
            int clipInFrame = targetFrame - Clip.StartFrame;
            ClipPlayable.SetTime((float)clipInFrame / TimelineUtility.FrameRate);
            PrepareFrame(default, default);

            //不混合rootmotion的位移量
            if (GetInputWeight() <= 0f) return;
            //TODO 怎么抛出事件...? 
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
            output.ConnectInput(index, handle, 0, 0);

            return clipPlayable;
        }
    }

    #endregion
}