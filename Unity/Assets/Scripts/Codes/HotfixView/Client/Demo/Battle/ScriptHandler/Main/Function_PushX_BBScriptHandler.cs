using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Function_PushX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PushX";
        }

        //HitPushX: 2000, 8;(StartVelocity, Friction)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"PushX: (?<StartVelocity>.*?), (?<Friction>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["StartVelocity"].Value, out long vel) || !long.TryParse(match.Groups["Friction"].Value,out long friction))
            {
                Log.Error($"cannot format {match.Groups["StartVelocity"].Value} / {match.Groups["Friction"].Value} to long!!");
                return Status.Failed;
            }

            parser.RemoveComponent<PushXComponent>();
            
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            
            //TODO: 关于PushX的方向，要考虑拉回和推远的情况
            //拉回: 艾德的鞭子
            //推远：拳脚打防，两方推远
            int direction = -body.GetFlip();
            float pushV = vel / 10000f;
            float pushF = friction / 10000f;
            parser.AddComponent<PushXComponent, int, float, float>(direction, pushV, pushF);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}