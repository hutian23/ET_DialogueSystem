namespace ET.Client
{
    [Invoke]
    public class HandleEndContactCallback : AInvokeHandler<EndContact>
    {
        public override void Handle(EndContact args)
        {
            Log.Warning("EndContact");
        }
    }
}