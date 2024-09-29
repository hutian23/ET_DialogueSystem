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
        }
    }
}