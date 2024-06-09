﻿using Timeline;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class RootMotionComponent: Entity, IAwake, IDestroy
    {
        // 当前behavior的初始位置
        public Vector3 initPos;

        //当前帧总位移
        public Vector3 moveCounter;

        public BBAnimationClip currentClip;
    }
}