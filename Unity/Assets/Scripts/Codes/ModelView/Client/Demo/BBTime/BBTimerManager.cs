using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBTimerManager : Entity,IAwake,IDestroy, IUpdate, IFrameUpdate, ILoad, IFrameLateUpdate
    {
        [StaticField]
        public static BBTimerManager Instance;
        public Stopwatch _gameTimer = new();
        public long LastTime;

        public long SceneTimer;
        public long LateUpdateTimer; // LateUpdate生命周期事件，相机移动等逻辑
        public Queue<long> instanceIds = new(); // UnitTimers
    }
}