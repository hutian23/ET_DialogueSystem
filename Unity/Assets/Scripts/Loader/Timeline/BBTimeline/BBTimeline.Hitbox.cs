using System;
using System.Collections.Generic;
using ET;
using ET.Client;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timeline
{
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

    [Color(165, 032, 025)]
    public class BBHitboxClip: BBClip
    {
        public Dictionary<int, List<BoxInfo>> boxInfoDict = new();

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
        CounterHurt,
        Throw,
        Squash,
        Proximity
    }

    [Serializable]
    public class BoxInfo
    {
        [LabelText("判定框名: ")]
        public string boxName;

        [LabelText("绑定对象: ")]
        [Sirenix.OdinInspector.ReadOnly]
        public string bindParent;

        [LabelText("判定框类型: ")]
        public HitboxType hitboxType;

        [LabelText("偏移: ")]
        public Vector2 center;

        [LabelText("大小: ")]
        public Vector2 size;
    }

    [Serializable]
    public class CastBoxInfo
    {
        [HideLabel]
        [HideReferenceObjectPicker]
        public BoxInfo info;

        [HideInInspector]
        public CastBox castBox;

        [HideInInspector]
        public BBHitboxInspectorData InspectorData;

        [Space(8)]
        [LabelText("绑定对象: ")]
        public GameObject go;

        public string boxName => info.boxName;

        public CastBoxInfo(CastBox _castBox, BBHitboxInspectorData inspectorData)
        {
            castBox = _castBox;
            InspectorData = inspectorData;

            info = castBox.info;
            go = castBox.gameObject;
        }

        [Sirenix.OdinInspector.Button("复制")]
        public void Copy()
        {
            var boxCopy = MongoHelper.Clone(info);
            BBTimelineSettings.GetSettings().CopyTarget = boxCopy;
        }

        [Sirenix.OdinInspector.Button("黏贴")]
        public void Paste()
        {
            if (BBTimelineSettings.GetSettings().CopyTarget is not BoxInfo infoCopy) return;
            info.size = infoCopy.size;
            info.center = infoCopy.center;
            BBTimelineSettings.GetSettings().CopyTarget = null;
        }

        [Sirenix.OdinInspector.Button("删除")]
        public void Delete()
        {
            Object.DestroyImmediate(castBox.gameObject);
            InspectorData.UpdateCastBoxInspector();
        }
    }

    [Serializable]
    public class BBHitboxInspectorData: ShowInspectorData
    {
        [Sirenix.OdinInspector.ReadOnly]
        [LabelText("当前帧: ")]
        public int ClipInFrame;

        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = true, ShowIndexLabels = true, ListElementLabelName = "boxName", IsReadOnly = true)]
        public List<CastBoxInfo> CastBoxInfos = new();

        #region Create

        private TimelineFieldView FieldView;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;
        private BBHitboxClip hitboxClip;

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
            //refer 
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            if (refer == null)
            {
                refer = timelinePlayer.gameObject.AddComponent<ReferenceCollector>();
            }

            refer.Remove(bindParent.name);
            refer.Add(bindParent.name, bindParent);

            //create hit prefab
            GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, bindParent.transform);
            go.name = boxName;

            CastBox castBox = go.GetComponent<CastBox>();
            BoxInfo info = new()
            {
                boxName = boxName,
                hitboxType = HitBoxType,
                center = Vector2.zero,
                size = Vector3.one,
                bindParent = bindParent.name
            };
            castBox.info = info;

            UpdateCastBoxInspector();
        }

        [Sirenix.OdinInspector.Button("清空")]
        public void ClearHitbox()
        {
            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }

            UpdateCastBoxInspector();
        }

        [Sirenix.OdinInspector.Button("移除")]
        public void DeleteHitbox()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() => { hitboxClip.boxInfoDict.Remove(ClipInFrame); }, "Delete Hitbox");
        }

        [Sirenix.OdinInspector.Button("保存")]
        public void SaveHitbox()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                if (!hitboxClip.boxInfoDict.ContainsKey(ClipInFrame))
                {
                    hitboxClip.boxInfoDict.TryAdd(ClipInFrame, new List<BoxInfo>());
                }

                hitboxClip.boxInfoDict.TryGetValue(ClipInFrame, out var infos);

                infos.Clear();
                foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
                {
                    var copyInfo = MongoHelper.Clone(castBox.info);
                    infos.Add(copyInfo);
                }
            }, "Save Hitbox");
        }

        #endregion

        public BBHitboxInspectorData(object target): base(target)
        {
            hitboxClip = target as BBHitboxClip;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
            UpdateCastBoxInspector();
        }

        public void UpdateCastBoxInspector()
        {
            CastBoxInfos.Clear();
            foreach (var castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                CastBoxInfo castBoxInfo = new(castBox, this);
                CastBoxInfos.Add(castBoxInfo);
            }
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            ClipInFrame = FieldView.GetCurrentTimeLocator() - hitboxClip.StartFrame;
            UpdateCastBoxInspector();
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }

#endif

    #region Runtime

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        private int currentFrame;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        public override void SetTime(int targetFrame)
        {
            //帧更新
            if (currentFrame == targetFrame) return;
            currentFrame = targetFrame;

            foreach (var clip in Track.Clips)
            {
                if (clip.InMiddle(targetFrame))
                {
                    BBHitboxClip hitboxClip = clip as BBHitboxClip;
                    int clipFrame = targetFrame - clip.StartFrame;
                    if (!hitboxClip.boxInfoDict.TryGetValue(clipFrame, out List<BoxInfo> boxInfos)) return;

                    //Clear hitboxes
                    foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
                    {
                        Object.DestroyImmediate(castBox.gameObject);
                    }
                    
                    //Create hitbox
                    ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
                    foreach (var boxInfo in boxInfos)
                    {
                        GameObject parent = refer.Get<GameObject>(boxInfo.bindParent);
                        GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, parent.transform);
                        go.name = boxInfo.boxName;
                        go.GetComponent<CastBox>().info = boxInfo;
                    }

                    return;
                }
            }

            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion
}