namespace ET.Client
{
    public class Function_MonsterWave_WaitClear_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MonsterWave_WaitClear";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            MonsterWaveComponent wave = parser.GetComponent<MonsterWaveComponent>();

            return await wave.WaitClear(token);
        }
    }
}