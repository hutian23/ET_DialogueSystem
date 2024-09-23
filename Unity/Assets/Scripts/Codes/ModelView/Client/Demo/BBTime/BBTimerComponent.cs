using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Client
{
    [Serializable]
    public class BBTimerAction
    {
        public long Id;
        public TimerClass TimerClass;
        public long startFrame;
        public object Object;
        public long Frame; // 持续的帧数
        public int Type; //战斗中的事件 具体见TODTimeInvokeType

        public static BBTimerAction Create(long id, TimerClass timerClass, long startFrame, long frame, int type, object obj)
        {
            BBTimerAction timerAction = ObjectPool.Instance.Fetch<BBTimerAction>();
            timerAction.Id = id;
            timerAction.TimerClass = timerClass;
            timerAction.startFrame = startFrame;
            timerAction.Object = obj;
            timerAction.Type = type;
            timerAction.Frame = frame;
            return timerAction;
        }

        public void Recycle()
        {
            this.Id = 0;
            this.TimerClass = TimerClass.None;
            this.Object = null;
            this.startFrame = 0;
            this.Frame = 0;
            this.Type = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public struct BBTimerCallback
    {
        public object Args;
    }

    [ComponentOf]
    public class BBTimerComponent: Entity, IAwake, IDestroy, ILoad, IUpdate
    {
        public readonly MultiMap<long, long> TimerId = new();

        public readonly Queue<long> timeOutTime = new();

        public readonly Queue<long> timeOutTimerIds = new();

        public readonly Dictionary<long, BBTimerAction> timerActions = new();

        public long idGenerator;

        // 记录最小事件，不用每次都去MultiMap取第一个值
        public long minFrame = long.MaxValue;
        public long curFrame = 0;

        //标准更新频率60fps
        public int Hertz = 60;
        public long LastTime;
        public long Accumulator;
        public Stopwatch _gameTimer = new();
        
        
        
    }
}