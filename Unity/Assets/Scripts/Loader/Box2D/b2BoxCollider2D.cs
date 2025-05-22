using System;
using Timeline;
using UnityEngine;

namespace ET
{
    public class b2BoxCollider2D : CastShapeBase
    {
        public BoxInfo info;

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            Matrix4x4 gizmosMatrixRecord = Gizmos.matrix;
            Color gizmosColorRecord = Gizmos.color;

            switch (info.hitboxType)
            {
                case HitboxType.Hit:
                    Gizmos.color = Color.red;
                    break;
                case HitboxType.Hurt:
                    Gizmos.color = Color.green;
                    break;
                case HitboxType.Squash:
                    Gizmos.color = Color.yellow;
                    break;
                case HitboxType.Throw:
                    Gizmos.color = Color.blue;
                    break;
                case HitboxType.Proximity:
                    Gizmos.color = Color.magenta;
                    break;
                case HitboxType.Other:
                    Gizmos.color = Color.gray;
                    break;
                case HitboxType.Gizmos:
                    Gizmos.color = Color.cyan;
                    break;
                default:
                    Gizmos.color = Color.white;
                    break;
            }
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireCube(info.center, info.size);
            Gizmos.color = gizmosColorRecord;
            Gizmos.matrix = gizmosMatrixRecord;
        }
#endif
    }
    
    [Serializable]
    public class BoxInfo
    {
        public string boxName;
        
        public LayerType layerType;
        public TagType tagType;
        public HitboxType hitboxType;
        public bool isTrigger;
        
        public Vector2 center;
        public Vector2 size = Vector2.one;
    }
    
    public struct FixtureData
    {
        //传入碰撞事件时调用的组件instanceId
        public long InstanceId;

        public FixtureType Type;
        public LayerType LayerType;
        public TagType TagType;
        public bool IsTrigger;
        
        //判定框信息(BoxInfo)
        public string Name;
        public HitboxType HitboxType;
        public Vector2 Center;
        public Vector2 Size;
        
        //碰撞事件
        public int TriggerEnterId;
        public int TriggerStayId;
        public int TriggerExitId;
        public int CollisionEnterId;
        public int CollisionStayId;
        public int CollisionExitId;

        public object UserData;
    }

    [Flags]
    public enum LayerType
    {
        None = 0, 
        Ground = 1 << 0,
        Unit = 1 << 1
    }

    public enum FixtureType
    {
        None = 0,
        Default = 1,
        Hitbox = 2
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
    
    [Flags]
    public enum TagType
    {
        None = 0,
        Wall = 1 << 0, 
        Ground = 1 << 1,
        Invincible = 1 << 2,
        BulletInvincible = 1 << 3
    }
}