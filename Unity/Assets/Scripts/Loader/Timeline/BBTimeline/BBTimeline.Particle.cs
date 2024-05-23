using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timeline
{
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
        public override Type ClipViewType => typeof (ParticleClipView);
#endif
    }

#if UNITY_EDITOR
    [Color(127, 214, 253)]
#endif
    public class BBParticleClip: BBClip
    {
        public string ParticleName;
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

    public class ParticleKeyframe
    {
        public Vector3 offset;
        public Vector3 rotation;
    }

    public class ParticleRuntimeTrack: RuntimeTrack
    {
        private ParticleSystem particle;
        private BBParticleClip currentClip;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        private int currentFrame;

        public ParticleRuntimeTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
            Dispose();
        }

        private void Dispose()
        {
            currentClip = null;
            if (particle == null) return;
            Object.DestroyImmediate(particle.gameObject);
        }

        private void UpdateParticle()
        {
            //当前帧在clip中的相对位置
            int currentClipInFrame = Mathf.Max(0, currentFrame - currentClip.StartFrame);

            if (particle == null) return;

            particle.Simulate((float)currentClipInFrame / TimelineUtility.FrameRate);
            if (!currentClip.keyframeDict.TryGetValue(currentClipInFrame, out var keyframe)) return;

            var gameObject = particle.gameObject;
            gameObject.transform.localPosition = keyframe.offset;
            gameObject.transform.localEulerAngles = keyframe.rotation;
        }

        public override void SetTime(int targetFrame)
        {
            if (currentFrame == targetFrame) return;
            currentFrame = targetFrame;

            foreach (BBClip clip in Track.Clips)
            {
                if (clip.InMiddle(targetFrame))
                {
                    BBParticleClip particleClip = clip as BBParticleClip;
                    if (particleClip == currentClip)
                    {
                        UpdateParticle();
                        return;
                    }

                    Dispose();

                    currentClip = particleClip;

                    if (particleClip.ParticlePrefab == null) return;

                    particle = Object.Instantiate(particleClip.ParticlePrefab, timelinePlayer.transform);
                    ParticleCollector collector = particle.gameObject.AddComponent<ParticleCollector>();
                    collector.Init(particleClip);

                    UpdateParticle();
                    return;
                }
            }

            Dispose();
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
        private int currentClipFrame;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [PropertyOrder(1)]
        public string ParticleName;

        [PropertyOrder(2)]
        public ParticleSystem ParticlePrefab;

        [PropertySpace(3)]
        [PropertyOrder(3), Sirenix.OdinInspector.Button("Rebind")]
        public void Rebind()
        {
            FieldView.EditorWindow.ApplyModify(() =>
            {
                Clip.ParticleName = ParticleName;
                Clip.ParticlePrefab = ParticlePrefab;
            }, "rebind particle clip");
        }

        private bool HasBind => ParticleObject != null;

        [PropertySpace(10), PropertyOrder(4), Sirenix.OdinInspector.ShowIf("HasBind")]
        public ParticleSystem ParticleObject;

        [PropertyOrder(5)]
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowIf("HasBind")]
        public Vector3 Offset;

        [PropertyOrder(6)]
        [Sirenix.OdinInspector.ReadOnly, Sirenix.OdinInspector.ShowIf("HasBind")]
        public Vector3 Rotation;

        [PropertyOrder(7), Sirenix.OdinInspector.Button("Record"),Sirenix.OdinInspector.ShowIf("HasBind")]
        public void Record()
        {
            if (ParticleObject == null) return;

            FieldView.EditorWindow.ApplyModify(() =>
            {
                Clip.keyframeDict.Remove(currentClipFrame);

                var transform = ParticleObject.transform;
                Clip.keyframeDict.Add(currentClipFrame,
                    new ParticleKeyframe() { offset = transform.localPosition, rotation = transform.localEulerAngles });
            }, "Record particle clip keyframe");
        }

        private void UpdateParticleObject()
        {
            //Find particle system 
            bool hasFound = false;
            foreach (ParticleCollector collector in timelinePlayer.GetComponentsInChildren<ParticleCollector>())
            {
                if (collector.particleName != Clip.ParticleName) continue;

                hasFound = true;
                ParticleSystem par = collector.GetComponent<ParticleSystem>();
                if (ParticleObject == par) break;
                ParticleObject = par;
            }

            if (!hasFound)
            {
                ParticleObject = null;
                return;
            }
            
            //Update transform
            var trans = ParticleObject.transform;
            Offset = trans.localPosition;
            Rotation = trans.localEulerAngles;
        }

        public ParticleClipInspectorData(object target): base(target)
        {
            Clip = target as BBParticleClip;
            ParticlePrefab = Clip.ParticlePrefab;
            ParticleName = Clip.ParticleName;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            EditorApplication.update += UpdateParticleObject;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            currentClipFrame = fieldView.GetCurrentTimeLocator() - Clip.StartFrame;
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
            EditorApplication.update -= UpdateParticleObject;
        }
    }
#endif
}