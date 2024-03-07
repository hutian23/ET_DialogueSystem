namespace ET.Client
{
    public class InitHP_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InitHP";
        }

        //InitHp: 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<NumericComponent>()[NumericType.MaxHpBase] = 30;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}