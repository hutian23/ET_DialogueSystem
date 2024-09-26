using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    public class HandlePausedModeCallback: AInvokeHandler<PausedCallback>
    {
        public override void Handle(PausedCallback args)
        {
            //pause b2world
            Global.Settings.Pause = args.Pause;
            
            //pause timeline
            //因为timeline的更新都是基于timercomponent,停止计时器
            TimelineManager.Instance.Pause(args.Pause);
        }
    }
}