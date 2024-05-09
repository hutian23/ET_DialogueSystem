using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;

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
        public class HitboxDictionary: UnitySerializedDictionary<int, HitboxInfo>
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
    public class TestHitboxInfo
    {
        [LabelText("判定框名: ")]
        public string boxName;

        [LabelText("判定框类型: ")]
        public HitboxType HitboxType;

        public Vector2 localPosition;
        public Vector2 scale;
    }

    public class BBHitboxInfo
    {
        [HideInInspector]
        public BBHitboxInspectorData inspectorData;

        private BBHitboxClip clip => inspectorData.hitboxClip;
        private TimelineFieldView FieldView => inspectorData.FieldView;
        private TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [LabelText("当前帧: ")]
        [DisableInEditorMode]
        public int frame;

        [Space(5)]
        [LabelText("对象: ")]
        public GameObject parent;

        [ButtonGroup]
        public void Bind()
        {
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            refer.Remove(parent.name);
            refer.Add(parent.name, parent);

            if (!clip.hitboxDictionary.TryGetValue(frame, out HitboxInfo info))
            {
                info = new HitboxInfo();
                clip.hitboxDictionary.Add(frame, info);
            }

            info.referName = parent.name;
        }

        [HideReferenceObjectPicker]
        public TestHitboxInfo hitboxInfo = new();

        public void Awake()
        {
            var refer = timelinePlayer.GetComponent<ReferenceCollector>();
            if (refer == null)
            {
                timelinePlayer.gameObject.AddComponent<ReferenceCollector>();
            }
        }

        public void Update()
        {
            //当前帧
            int currentFrame = FieldView.GetCurrentTimeLocator() - inspectorData.hitboxClip.StartFrame;
            frame = currentFrame;

            if (!clip.hitboxDictionary.TryGetValue(currentFrame, out HitboxInfo info))
            {
                parent = null;
                return;
            }

            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            GameObject go = refer.Get<GameObject>(info.referName);
            parent = go;
        }
    }

    [Serializable]
    public class BBHitboxInspectorData: IShowInInspector
    {
        public BBHitboxClip hitboxClip;
        public TimelineInspectorData inspectorData;
        public BBHitboxInfo hitboxInfo;

        public TimelineFieldView FieldView;

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            fieldView.ClipInspector.Clear();
            hitboxInfo = new BBHitboxInfo();
            inspectorData = TimelineInspectorData.CreateView(fieldView.ClipInspector, hitboxInfo);

            hitboxInfo.inspectorData = this;
            hitboxInfo.Awake();
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            hitboxInfo.Update();
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