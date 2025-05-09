using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(b2Body))]
    public class HitEvent_HitStop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //HitStop: 6, 8;(Hertz, hitStopFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<Hertz>.*?), (?<HitStop>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Hertz"].Value, out int hertz))
            {
                Log.Error($"cannot format Hertz to int!");
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }

            //1. 获取攻击的碰撞信息
            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();

            //2. 获取受击方的Hertz组件
            b2Body _body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;

            Unit _unit = _body.GetParent<Unit>();
            Unit unit = parser.GetParent<Unit>();
            BBParser _parser = _unit.GetComponent<BBParser>();
            
            HertzAbility _ability = _unit.GetComponent<BuffManager>().GetComponent<HertzAbility>();
            HertzAbility ability = unit.GetComponent<BuffManager>().GetComponent<HertzAbility>();
            
            //3. 添加HitStop Buff
            _parser.RemoveComponent<TimeFrozeComponent>();
            _parser.AddComponent<TimeFrozeComponent, int, int, long>(hertz, hitStop, _ability.InstanceId);
            parser.RemoveComponent<TimeFrozeComponent>();
            parser.AddComponent<TimeFrozeComponent, int, int, long>(hertz, hitStop, ability.InstanceId);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}