using System;
using System.Collections.Generic;
using Timeline.Editor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timeline
{
    [Serializable]
    [BBTrack("TargetBind")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("51d6e4824d3138c4880ca6308fa0e473")]
#endif
    public class BBTargetBindTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeTargetBindTrack);

#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBTargetBindClip);
        public override Type ClipViewType => typeof (TargetBindClipView);
#endif
    }

#if UNITY_EDITOR
    [Color(100, 100, 100)]
#endif
    public class BBTargetBindClip: BBClip
    {
        public string referName;
        public Dictionary<int, Vector3> TargetKeyframeDict = new();

        public BBTargetBindClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBTargetBindInspectorData);
#endif
    }

    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        private BBTargetBindClip currentClip;
        private GameObject targetBindGo;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        public RuntimeTargetBindTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
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
            foreach (BBClip clip in Track.Clips)
            {
                if (!clip.InMiddle(targetFrame)) continue;

                BBTargetBindClip targetBindClip = clip as BBTargetBindClip;
                if (currentClip != targetBindClip)
                {
                    //Editor阶段生成go 录制位置
                    if (targetBindGo != null) Object.DestroyImmediate(targetBindGo);
                    targetBindGo = new GameObject(targetBindClip.referName);
                    TargetBindCollector collector = targetBindGo.AddComponent<TargetBindCollector>(); //Dispose时移除该子物体
                    collector.targetBindName = targetBindClip.referName;
                    targetBindGo.transform.SetParent(timelinePlayer.transform);
                }

                currentClip = targetBindClip;

                //有无关键帧
                // int clipInFrame = targetFrame - targetBindClip.StartFrame;
                // if (!targetBindClip.TargetKeyframeDict.TryGetValue(clipInFrame, out var localPos)) return;
                return;
            }

            if (targetBindGo != null) Object.DestroyImmediate(targetBindGo);
            currentClip = null;
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion

    #region Editor

    [Serializable]
    public class BBTargetBindInspectorData: ShowInspectorData
    {
        private BBTargetBindClip targetBindClip;
        private TimelineFieldView FieldView;
        private TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        private TimelinePlayer timelinePlayer => EditorWindow.TimelinePlayer;

        public GameObject targetBindGameObject;
        public string targetBindName;

        [Sirenix.OdinInspector.Button("Rebind")]
        public void Rebind()
        {
            EditorWindow.ApplyModify(() => { targetBindClip.referName = targetBindName; }, "Update Targetbind name");
        }

        private bool BindGo => targetBindGameObject != null;
        [Sirenix.OdinInspector.Button("Record"), Sirenix.OdinInspector.ShowIf("BindGo")]
        public void Record()
        {
            EditorWindow.ApplyModify(() =>
            {
                int clipInFrame = FieldView.GetCurrentTimeLocator() - targetBindClip.StartFrame;
                Vector3 localPos = targetBindGameObject.transform.localPosition;
                targetBindClip.TargetKeyframeDict.Remove(clipInFrame);
                targetBindClip.TargetKeyframeDict.Add(clipInFrame, localPos);
            }, "Record keyframe");
        }

        private void UpdateGo()
        {
            foreach (var targetBindCollector in timelinePlayer.GetComponentsInChildren<TargetBindCollector>())
            {
                if (targetBindCollector.targetBindName != targetBindClip.referName) continue;
                targetBindGameObject = targetBindCollector.gameObject;
                return;
            }

            targetBindGameObject = null;
        }

        public BBTargetBindInspectorData(object target): base(target)
        {
            targetBindClip = target as BBTargetBindClip;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            targetBindName = targetBindClip.referName;
            UpdateGo();
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            UpdateGo();
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }

    #endregion
}