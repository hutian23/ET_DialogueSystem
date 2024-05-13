using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using ET.Client;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Timeline
{
    [Serializable]
    [BBTrack("Hitbox")]
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
    public class BBHitboxTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeHitboxTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBHitboxClip);
        public override Type ClipViewType => typeof (HitboxClipView);
#endif
    }

    [Serializable]
    public class HitboxInfo
    {
        public Rect hitBoxRect;

        //绑定对象 对应referenceCollector
        public string referName;
    }

    [Color(165, 032, 025)]
    public class BBHitboxClip: BBClip
    {
        [Serializable]
        public class HitboxDictionary: UnitySerializedDictionary<int, List<BoxInfo>>
        {
        }

        public HitboxDictionary hitboxDictionary = new();

        public BBHitboxClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BBHitboxInspectorData);
#endif
    }

#if UNITY_EDITOR
    public enum HitboxType
    {
        Hit,
        Hurt,
        Throw,
        Squash,
        Proximity
    }

    [Serializable]
    public class BoxInfo
    {
        public string boxName;
        public HitboxType hitboxType;
        public Vector2 center;
        public Vector2 size;
    }

    [Serializable]
    public class HitboxInfoInspector
    {
        [HideInInspector]
        public TimelineFieldView FieldView;

        [HideInInspector]
        public BBHitboxClip Clip;

        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;
        private BBTimeline timeline => timelinePlayer.RuntimeimePlayable.Timeline;

        [LabelText("当前帧: ")]
        [Sirenix.OdinInspector.ReadOnly]
        public int frame;
        
        #region Create

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框名: ")]
        public string boxName;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框类型: ")]
        public HitboxType HitBoxType;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("绑定对象: ")]
        public GameObject bindParent;

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("创建")]
        public void CreateHitbox()
        {
            GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, bindParent.transform);
            go.name = boxName;

            CastBox castBox = go.GetComponent<CastBox>();
            BoxInfo info = new() { boxName = boxName, hitboxType = HitBoxType, center = Vector2.zero, size = Vector3.one };
            castBox.info = info;
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("保存当前判定框")]
        public void SaveHitbox()
        {
            if (Clip.hitboxDictionary.ContainsKey(frame))
            {
                Clip.hitboxDictionary.Remove(frame);
            }
            
            var castBoxes = timelinePlayer.GetComponentsInChildren<CastBox>();
            if (castBoxes.Length == 0) return;

            List<BoxInfo> boxInfos = new();
            foreach (CastBox castBox in castBoxes)
            {
                BoxInfo boxInfo = MongoHelper.Clone(castBox.info);
                boxInfos.Add(boxInfo);
            }

            Clip.hitboxDictionary.TryAdd(frame, boxInfos);
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("清空判定框")]
        public void ClearHitbox()
        {
            foreach (var castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("更新判定框")]
        public void UpdateHitbox()
        {
            ClearHitbox();
        }

        #endregion
    }

    [Serializable]
    public class BBHitboxInspectorData: IShowInInspector
    {
        public BBHitboxClip hitboxClip;
        public TimelineInspectorData inspectorData;

        [FormerlySerializedAs("hitboxInfo")]
        public HitboxInfoInspector hitboxInfoInspector;

        public TimelineFieldView FieldView;

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            fieldView.ClipInspector.Clear();
            hitboxInfoInspector = new HitboxInfoInspector();
            inspectorData = TimelineInspectorData.CreateView(fieldView.ClipInspector, hitboxInfoInspector);
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            var inspector = hitboxInfoInspector;
            inspector.FieldView = fieldView;
            inspector.Clip = hitboxClip;
            inspector.frame = fieldView.GetCurrentTimeLocator() - hitboxClip.StartFrame;
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
            fieldView.ClipInspector.Clear();
        }

        public override bool Equal(object target)
        {
            // return hitboxClip == target as BBHitboxClip;
            return true;
        }

        public BBHitboxInspectorData(object target): base(target)
        {
            hitboxClip = target as BBHitboxClip;
        }
    }

#endif

    #region Runtime

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        public List<GameObject> HitboxGameObjects = new();

        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
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
}