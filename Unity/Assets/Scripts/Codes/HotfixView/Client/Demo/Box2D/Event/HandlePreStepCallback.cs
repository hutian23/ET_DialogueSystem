namespace ET.Client
{
    [Invoke]
    public class HandlePreStepCallback : AInvokeHandler<PreStepCallback>
    {
        public override void Handle(PreStepCallback args)
        {
            EventSystem.Instance.PreStepUpdate();
        }
    }
}