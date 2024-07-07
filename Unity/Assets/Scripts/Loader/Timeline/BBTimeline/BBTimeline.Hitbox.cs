using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [BBTrack("Hitbox")]
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
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
            int maxFrame = 0;
            foreach (HitboxKeyframe keyframe in Keyframes)
            {
                if (keyframe.frame >= maxFrame)
                {
                    maxFrame = keyframe.frame;
                }
            }

            return maxFrame;
        }
#endif
    }

    [Serializable]
    public class HitboxKeyframe
    {
        [ReadOnly]
        public int frame;

        [HideReferenceObjectPicker]
        public List<BoxInfo> boxInfos = new();
    }

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

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        private int currentFrame = -1;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

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
#if UNITY_EDITOR
            if (currentFrame == targetFrame)
            {
                return;
            }

            currentFrame = targetFrame;

            BBHitboxTrack hitboxTrack = Track as BBHitboxTrack;
            foreach (HitboxKeyframe keyframe in hitboxTrack.Keyframes)
            {
                if (keyframe.frame != currentFrame)
                {
                    continue;
                }

                GenerateHitbox(timelinePlayer, keyframe);
                break;
            }
#endif
        }

#if UNITY_EDITOR
        public static void GenerateHitbox(TimelinePlayer timelinePlayer, HitboxKeyframe keyframe)
        {
            timelinePlayer.ClearTimelineGenerate();
            foreach (BoxInfo boxInfo in keyframe.boxInfos)
            {
                GameObject parent = timelinePlayer
                        .GetComponent<ReferenceCollector>()
                        .Get<GameObject>(boxInfo.hitboxType.ToString());

                GameObject child = new(boxInfo.boxName);
                child.transform.SetParent(parent.transform);
                child.AddComponent<TimelineGenerate>();
                CastBox castBox = child.AddComponent<CastBox>();
                castBox.info = boxInfo;
            }
        }
    }
#endif

    #endregion
}