using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(TimeFrozeComponent))]
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

            //1. 查询双方unit
            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyA = boxA.GetParent<b2Body>();
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitA = Root.Instance.Get(bodyA.unitId) as Unit;
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;

            //2. 双方都添加HitStop Buff
            BBParser parserA = unitA.GetComponent<BBParser>();
            BBParser parserB = unitB.GetComponent<BBParser>();

            parserA.RemoveComponent<TimeFrozeComponent>();
            parserB.RemoveComponent<TimeFrozeComponent>();
            TimeFrozeComponent hitStopA = parserA.AddComponent<TimeFrozeComponent>(true);
            TimeFrozeComponent hitStopB = parserB.AddComponent<TimeFrozeComponent>(true);

            //3. buff初始化
            hitStopA.Hertz = hertz;
            hitStopA.LastFrame = hitStop;
            hitStopA.unitId = unitA.InstanceId;

            hitStopB.Hertz = hertz;
            hitStopB.LastFrame = hitStop;
            hitStopB.unitId = unitB.InstanceId;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}