using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class MonsterWaveComponent : Entity, IAwake, IDestroy
    {
        // 管理monsterWaveFlag.instanceId, 队列为空时，表示当前波次怪物全部消灭
        public Queue<long> monsterQueue = new();
    }
}