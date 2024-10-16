﻿using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class BBAnimComponent: Entity, IAwake, IDestroy, ILoad
    {
        public HashSet<GameObject> hitBoxes = new();

        public ETCancellationToken token = new(); //取消当前播放的动画协程

        public Dictionary<string, BBKeyframe> keyFrameDict = new();
    }
}