using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandleContactFilterCallback : AInvokeHandler<ContactFilterCallback, bool>
    {
        public override bool Handle(ContactFilterCallback args)
        {
            return true;
        }
    }
}