using Timeline;
using UnityEngine;

namespace ET.Client
{
    // 缓存怪物unit.instanceId，方便查询
    public class Thrones_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_Init";
        }
        
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 添加依赖的组件
            Unit unit = parser.GetParent<Unit>();
            unit.RemoveComponent<BBTimerComponent>();
            unit.AddComponent<BBTimerComponent>().IsUnitTimer();
            
            //2. 缓存子unit的instanceId
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            ReferenceCollector refer = go.GetComponent<ReferenceCollector>();
            
            GameObject throne_1 = refer.Get<GameObject>("Throne_1");
            parser.RegistParam("Throne_1", throne_1.GetComponent<TimelinePlayer>().instanceId);
            
            GameObject throne_2 = refer.Get<GameObject>("Throne_2");
            parser.RegistParam("Throne_2", throne_2.GetComponent<TimelinePlayer>().instanceId);
            
            GameObject throne_3 = refer.Get<GameObject>("Throne_3");
            parser.RegistParam("Throne_3", throne_3.GetComponent<TimelinePlayer>().instanceId);

            await TimerComponent.Instance.WaitAsync(100, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}