namespace ET.Client
{
    // 战术时间 场景中的怪物减速 玩家和玩家生成的子弹不减速
    [ComponentOf(typeof(BuffManager))]
    public class TacticalTimeBuff : Entity, IAwake<int, int>, IDestroy, IFrameUpdate, IAwake
    {
        public int lastFrame;
        public int cnt;
        public int Hertz;
    }
}