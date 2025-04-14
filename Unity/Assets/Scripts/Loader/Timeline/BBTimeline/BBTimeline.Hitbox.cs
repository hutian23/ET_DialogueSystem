using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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

        public HitboxKeyframe GetClosestKeyframe(int targetFrame)
        {
            int closestFrame = -1;
            foreach (HitboxKeyframe keyFrame in Keyframes)
            {
                if (keyFrame.frame == targetFrame)
                {
                    closestFrame = keyFrame.frame;
                    break;
                }

                if (keyFrame.frame < targetFrame && targetFrame - keyFrame.frame < targetFrame - closestFrame)
                {
                    closestFrame = keyFrame.frame;
                }
            }

            return closestFrame == -1? null : GetKeyframe(closestFrame);
        }

#if UNITY_EDITOR
        public override int GetMaxFrame()
        {
            int max = 1;
            foreach (HitboxKeyframe keyframe in Keyframes)
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
        Other,
        Gizmos
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

    //有点套娃 --- > Fixture.UserData ---> FixtureData ---> UserData
    public struct FixtureData
    {
        //传入碰撞事件时调用的组件instanceId
        public long InstanceId;
        public string Name;
        public int Type;

        //碰撞事件
        public int TriggerEnterId;
        public int TriggerStayId;
        public int TriggerExitId;

        public int CollisionEnterId;
        public int CollisionStayId;
        public int CollisionExitId;

        public long LayerMask;
        public bool IsTrigger;

        public object UserData;
    }

    public static class LayerType
    {
        public const int None = 0;
        public const int Ground = 2 << 0;
        public const int Unit = 2 << 1;
        public const int Camera = 2 << 2;
    }

    public static class FixtureType
    {
        public const int None = 0;
        public const int Default = 1;
        public const int Hitbox = 2;
        public const int AirCheckBox = 3;
    }

    public struct UpdateHitboxCallback
    {
        public long instanceId;

        public HitboxKeyframe Keyframe;
    }

    public class RuntimeHitboxTrack: RuntimeTrack
    {
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
            ClearHitbox(timelinePlayer);
#endif
        }

        public override void SetTime(int targetFrame)
        {
            BBHitboxTrack hitboxTrack = Track as BBHitboxTrack;
            if (timelinePlayer.HasBindUnit)
            {
                HitboxKeyframe _keyFrame = hitboxTrack.GetKeyframe(targetFrame);
                if (_keyFrame == null)
                {
                    return;
                }

                EventSystem.Instance.Invoke(new UpdateHitboxCallback() { instanceId = timelinePlayer.instanceId, Keyframe = _keyFrame });
            }
            else
            {
                HitboxKeyframe _keyFrame = hitboxTrack.GetClosestKeyframe(targetFrame);
                if (_keyFrame == null)
                {
                    return;
                }
                GenerateHitbox(timelinePlayer, _keyFrame);
            }
        }

        private static void ClearHitbox(TimelinePlayer timelinePlayer)
        {
#if UNITY_EDITOR
            //1. 销毁HitboxTrack运行时产生的组件
            var goSet = new HashSet<GameObject>();
            foreach (Component component in timelinePlayer.GetComponentsInChildren<Component>())
            {
                if (component.GetComponent<CastBox>() != null)
                {
                    goSet.Add(component.gameObject);
                }
            }

            foreach (GameObject go in goSet)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
#endif
        }

        public static void GenerateHitbox(TimelinePlayer timelinePlayer, HitboxKeyframe keyframe)
        {
#if UNITY_EDITOR
            //1. 销毁HitboxTrack运行时产生的组件
            ClearHitbox(timelinePlayer);

            //2. 根据keyframe生成新的CastBox
            foreach (BoxInfo boxInfo in keyframe.boxInfos)
            {
                if (boxInfo.hitboxType is HitboxType.None)
                {
                    continue;
                }

                GameObject parent = timelinePlayer
                        .GetComponent<ReferenceCollector>()
                        .Get<GameObject>(boxInfo.hitboxType.ToString());

                GameObject child = new(boxInfo.boxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                //深拷贝
                CastBox castBox = child.AddComponent<CastBox>();
                castBox.info = MongoHelper.Clone(boxInfo);
            }
#endif
        }
    }
}