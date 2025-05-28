namespace ET.Client
{
    public class Function_MonsterWave_RegistEnemy_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MonsterWave_RegistEnemy";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            long instanceId = parser.GetParam<long>("SpawnEnemy_InstanceId");
            Unit enemy = Root.Instance.Get(instanceId) as Unit;

            MonsterWaveComponent wave = parser.GetComponent<MonsterWaveComponent>();
            DeathAbility ability = enemy.GetComponent<BuffManager>().GetComponent<DeathAbility>();
            MonsterWaveFlag waveFlag = ability.AddComponent<MonsterWaveFlag>(true);

            wave.RegistEnemy(waveFlag);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}