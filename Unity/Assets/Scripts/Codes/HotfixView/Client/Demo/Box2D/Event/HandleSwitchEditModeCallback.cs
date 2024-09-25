using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    public class HandleSwitchEditModeCallback: AInvokeHandler<SwitchEditModeCallback>
    {
        public override void Handle(SwitchEditModeCallback args)
        {
            //pause b2world
            Global.Settings.Pause = !args.IsEdit;
            
            //pause timeline
            //因为timeline的更新都是基于timercomponent,将hertz设为0
            
            Log.Warning(args.IsEdit.ToString());
        }
    }
}