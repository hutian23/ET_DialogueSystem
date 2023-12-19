using System.Collections.Generic;

namespace ET.Client
{
    public class TODTimerAction
    {
        public long Id;
        public TimerClass TimerClass;
        public long startFrame;
        public object Object;
        public long Frame; // 持续的帧数
        public int Type; //战斗中的事件 具体见TODTimeInvokeType

        public static TODTimerAction Create(long id, TimerClass timerClass, long startFrame, long frame, int type, object obj)
        {
            TODTimerAction timerAction = ObjectPool.Instance.Fetch<TODTimerAction>();
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

    public struct TODTimerCallback
    {
        public object Args;
    }
    
    [ComponentOf]
    public class TODTimerComponent: Entity, IAwake, IDestroy, ILoad, IUpdate
    {
        public readonly MultiMap<long, long> TimerId = new();

        public readonly Queue<long> timeOutTime = new();

        public readonly Queue<long> timeOutTimerIds = new();

        public readonly Dictionary<long, TODTimerAction> timerActions = new();

        public long idGenerator;

        // 记录最小事件，不用每次都去MultiMap取第一个值
        public long minFrame = long.MaxValue;
        public long curFrame = 0;
        public float deltaTimereminder = 0;
        
        public float timeScale = 1f;
    }
}