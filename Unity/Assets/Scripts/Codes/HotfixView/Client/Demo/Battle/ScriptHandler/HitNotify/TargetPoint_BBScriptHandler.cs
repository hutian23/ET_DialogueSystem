using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class TargetPoint_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TargetPoint";
        }

        //ThrowPoint: -1400, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"TargetPoint: (?<BindX>-?\d+), (?<BindY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["BindX"].Value, out long x) || !long.TryParse(match.Groups["BindY"].Value, out long y))
            {
                Log.Error($"cannot format {match.Groups["BindX"].Value} or {match.Groups["BindY"].Value} to long!!");
                return Status.Failed;
            }

            if (!parser.ContainParam("TargetBind")) return Status.Failed;
            
            long bodyId = parser.GetParam<long>("TargetBind");
            b2Body bodyA = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            b2Body bodyB = Root.Instance.Get(bodyId) as b2Body;

            Vector2 BindPos = bodyA.GetPosition() + new Vector2(-bodyA.GetFlip() * (x / 10000f), y / 10000f);
            bodyB.SetPosition(BindPos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}