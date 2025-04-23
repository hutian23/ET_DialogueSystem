namespace ET.Client
{
    public class Function_Break_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Break";
        }

        //结束while循环
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //销毁Loop组件
            parser.RemoveComponent<LoopComponent>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}