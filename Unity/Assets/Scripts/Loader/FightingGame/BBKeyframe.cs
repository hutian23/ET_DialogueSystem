using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Serializable]
    public struct BBKeyframe
    {
        //关键帧帧号
        public long frameNo;
        //当前帧 动画
        public Sprite sprite;
        public List<HitBoxInfo> hitBoxInfos;
    }

    [Serializable]
    public struct HitBoxInfo
    {
        public string name;
        public HitBoxType type;
        public Rect rect;
        public HashSet<int> tags;
    }
    
    public enum HitBoxType
    {
        None,
        HitBox,
        HurtBox ,
        ThrowBox,
        ThrowHurtBox,
        PushBox, 
        ProximityBox//预防御
    }
}