namespace ET.Client
{
    public class Function_MonsterWave_Init_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MonsterWave_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.RemoveComponent<MonsterWaveComponent>();
            parser.AddComponent<MonsterWaveComponent>(true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}