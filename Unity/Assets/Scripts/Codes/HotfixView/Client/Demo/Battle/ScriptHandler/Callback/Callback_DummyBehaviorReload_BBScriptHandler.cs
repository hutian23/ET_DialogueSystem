namespace ET.Client
{
    public class Callback_DummyBehaviorReload_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DummyBehaviorReload";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            
            Transition transition = unit.GetComponent<Transition>();
            transition.CacheFlag();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}