namespace ET.Client
{
    [Invoke]
    public class HandleBeginContactCallback : AInvokeHandler<BeginContact>
    {
        public override void Handle(BeginContact args)
        {
            Log.Warning("BeginContact");
        }
    }
}