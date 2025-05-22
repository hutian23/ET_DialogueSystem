using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandlePostStepCallback : AInvokeHandler<PostStepCallback>
    {
        public override void Handle(PostStepCallback args)
        {
            EventSystem.Instance.PostStepUpdate();
        }
    }
}