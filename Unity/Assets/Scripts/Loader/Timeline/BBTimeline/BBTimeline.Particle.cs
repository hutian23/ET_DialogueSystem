using System;
using System.Collections.Generic;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("Particle")]
#if UNITY_EDITOR
    [Color(127, 214, 253)]
    [IconGuid("348da3c2f85477f4594cabc88bc48a84")]
#endif
    public class BBParticleTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (ParticleRuntimeTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBParticleClip);
        public override Type ClipViewType => typeof (TimelineClipView);
#endif
    }

#if UNITY_EDITOR
    [Color(127, 214, 253)]
#endif
    public class BBParticleClip: BBClip
    {
        public ParticleSystem ParticlePrefab;
        public Dictionary<int, ParticleKeyframe> keyframeDict = new();

        public BBParticleClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (ParticleClipInspectorData);
#endif
    }

    #region Runtime
    [Serializable]
    public class ParticleKeyframe
    {
        public Vector2 offset;
        public Vector2 rotation;
    }

    public class ParticleRuntimeTrack: RuntimeTrack
    {
        public ParticleRuntimeTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion

#if UNITY_EDITOR
    [Serializable]
    public class ParticleClipInspectorData: ShowInspectorData
    {
        private BBParticleClip Clip;
        private TimelineFieldView FieldView;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [Sirenix.OdinInspector.OnValueChanged("Rebind")]
        public ParticleSystem ParticlePrefab;

        public void Rebind()
        {
            FieldView.EditorWindow.ApplyModify(() => { Clip.ParticlePrefab = ParticlePrefab; }, "rebind particle");
        }

        public ParticleClipInspectorData(object target): base(target)
        {
            Clip = target as BBParticleClip;
            ParticlePrefab = Clip.ParticlePrefab;
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
#endif
}