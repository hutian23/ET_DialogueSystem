using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [BBTrack("Hitbox")]
#if UNITY_EDITOR
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
#endif
    public class BBHitboxTrack: BBTrack
    {
        [OdinSerialize, NonSerialized]
        public List<HitboxKeyframe> Keyframes = new();

        public override Type RuntimeTrackType => typeof (RuntimeHitboxTrack);

        public HitboxKeyframe GetKeyframe(int targetFrame)
        {
            foreach (HitboxKeyframe keyframe in Keyframes)
            {
                if (keyframe.frame == targetFrame)
                {
                    return keyframe;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        public override Type TrackViewType => typeof (HitboxTrackView);
        public override int GetMaxFrame()
        {
            var max = 1;
            foreach (var keyframe in Keyframes)
            {
                if (keyframe.frame >= max)
                {
                    max = keyframe.frame;
                }
            }

            return max;
        }
#endif
    }

    [Serializable]
    public class HitboxKeyframe: BBKeyframeBase
    {
        [HideReferenceObjectPicker]
        public List<BoxInfo> boxInfos = new();
    }

    public enum HitboxType
    {
        None,
        Hit,
        Hurt,
        Throw,
        Squash,
        Proximity,
        Other
    }

    [Serializable]
    public class BoxInfo
    {
        [LabelText("判定框名: ")]
        public string boxName;

        [LabelText("判定框类型: ")]
        public HitboxType hitboxType;

        [LabelText("偏移: ")]
        public Vector2 center;

        [LabelText("大小: ")]
        public Vector2 size = Vector2.one;
    }

#if UNITY_EDITOR
    [Serializable]
    public class HitboxMarkerInspectorData: ShowInspectorData
    {
        [HideReferenceObjectPicker]
        [HideLabel]
        public HitboxKeyframe Keyframe;

        private TimelineFieldView fieldView;

        [Button("刷新")]
        private void Refresh()
        {
            RuntimeHitboxTrack.GenerateHitbox(fieldView.EditorWindow.TimelinePlayer, Keyframe);
        }

        [Button("保存")]
        private void Save()
        {
            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() => { }, "Save hitbox");
        }

        public HitboxMarkerInspectorData(object target): base(target)
        {
            Keyframe = target as HitboxKeyframe;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }
#endif

    #region Runtime

    public struct UpdateHitboxCallback
    {
        public long instanceId;

        public HitboxKeyframe Keyframe;
    }

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        private int currentFrame = -1;

        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
#if UNITY_EDITOR
            timelinePlayer.ClearTimelineGenerate();
#endif
        }

        public override void SetTime(int targetFrame)
        {
            BBHitboxTrack hitboxTrack = Track as BBHitboxTrack;
            foreach (HitboxKeyframe keyframe in hitboxTrack.Keyframes)
            {
                if (keyframe.frame != targetFrame)
                {
                    continue;
                }

                //Hitbox没有发生更新
                if (currentFrame == targetFrame)
                {
                    break;
                }

                currentFrame = targetFrame;

                if (timelinePlayer.HasBindUnit)
                {
                    EventSystem.Instance.Invoke(new UpdateHitboxCallback() { instanceId = timelinePlayer.instanceId, Keyframe = keyframe });
                }
#if UNITY_EDITOR
                GenerateHitbox(timelinePlayer, keyframe);
#endif
                break;
            }
        }

#if UNITY_EDITOR
        public static void GenerateHitbox(TimelinePlayer timelinePlayer, HitboxKeyframe keyframe)
        {
            timelinePlayer.ClearTimelineGenerate();
            foreach (BoxInfo boxInfo in keyframe.boxInfos)
            {
                
                if(boxInfo.hitboxType is HitboxType.None) continue;
                
                GameObject parent = timelinePlayer
                        .GetComponent<ReferenceCollector>()
                        .Get<GameObject>(boxInfo.hitboxType.ToString());

                GameObject child = new(boxInfo.boxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                child.AddComponent<TimelineGenerate>();
                CastBox castBox = child.AddComponent<CastBox>();
                castBox.info = boxInfo;
            }
        }
    }
#endif

    #endregion
}