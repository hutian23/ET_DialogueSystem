using System.Collections.Generic;

namespace ET.Client
{
    // 部分特效，在玩家执行完当前行为时，需要全部销毁
    [ComponentOf(typeof(BBParser))]
    public class SkillEffectManager : Entity, IAwake, IDestroy
    {
        // key: Effect.Name Value: Effect.id
        public Dictionary<string, long> effectDict = new();
    }
}