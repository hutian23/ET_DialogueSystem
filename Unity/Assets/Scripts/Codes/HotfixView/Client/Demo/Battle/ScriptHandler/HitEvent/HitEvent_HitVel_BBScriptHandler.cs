using System.Numerics;
using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_HitVel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitVel";
        }

        //HitVel: 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitVel: (?<Vel_X>.*?), (?<Vel_Y>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Vel_X"].Value, out long x) ||
                !long.TryParse(match.Groups["Vel_Y"].Value, out long y))
            {
                Log.Error($"{match.Groups["Vel_X"].Value} / {match.Groups["Vel_Y"].Value}");
                return Status.Failed;
            }

            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();

            b2Body _body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            _body.SetVelocity(new Vector2(x, y) / 10000f);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}