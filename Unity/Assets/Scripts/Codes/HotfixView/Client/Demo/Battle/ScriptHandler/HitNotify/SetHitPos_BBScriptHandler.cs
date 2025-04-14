using System.Numerics;
using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    public class SetHitPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetHitPos";
        }

        //SetHitPos: 10000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetHitPos: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX) ||
                !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format {match.Groups["posX"].Value} / {match.Groups["posY"].Value} to long!!");
                return Status.Failed;
            }

            CollisionInfo info = parser.GetParam<CollisionInfo>("HitNotify_CollisionInfo");
            b2Body bodyA = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            
            Vector2 pos = new Vector2(posX, posY) / 10000f;
            bodyB.SetPosition(pos + bodyA.GetPosition());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}