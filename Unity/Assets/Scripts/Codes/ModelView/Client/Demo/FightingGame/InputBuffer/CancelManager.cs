using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class CancelManager: Entity, IAwake, IDestroy
    {
        public long Timer;

        //SkillInfo行为配置组件Id
        public long infoId;
        //保存当前帧的取消关系
        public Dictionary<int, bool> CancelableDict = new();

        public HashSet<int> GcOptions = new();
    }
}