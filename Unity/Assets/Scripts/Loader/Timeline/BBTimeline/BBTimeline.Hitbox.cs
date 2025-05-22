using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using ET;

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
            var goSet = new HashSet<GameObject>();
            foreach (TimelineObject timelineObject in timelinePlayer.GetComponentsInChildren<TimelineObject>())
            {
                if (timelineObject.GetComponent<b2BoxCollider2D>() != null)
                {
                    goSet.Add(timelineObject.gameObject);
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

            //2. 根据keyframe生成新的b2BoxCollider2D
            foreach (BoxInfo boxInfo in keyframe.boxInfos)
            {
                if (boxInfo.hitboxType is HitboxType.None)
                {
                    continue;
                }
            
                //3. 生成子GameObject
                GameObject parent = timelinePlayer.GetComponent<ReferenceCollector>().Get<GameObject>(boxInfo.hitboxType.ToString());
                GameObject child = new(boxInfo.boxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                child.AddComponent<TimelineObject>(); // 标注这是Timeline生成的GameObject
                
                //4. 深拷贝BoxInfo数据到Box中
                b2BoxCollider2D box = child.AddComponent<b2BoxCollider2D>();
                box.info = MongoHelper.Clone(boxInfo);
            }
#endif
        }
    }
}