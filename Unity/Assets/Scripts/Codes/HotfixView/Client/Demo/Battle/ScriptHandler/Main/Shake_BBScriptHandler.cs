using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(ShakeComponent))]
    public class Shake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Shake";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength_X"].Value} / {match.Groups["ShakeLength_Y"].Value} / {match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            parser.RemoveComponent<ShakeComponent>();

            //1. 初始化数值
            ShakeComponent shakeComponent = parser.AddComponent<ShakeComponent>();
            shakeComponent.shakeLength_X = shakeLength_X / 10000f;
            shakeComponent.shakeLength_Y = shakeLength_Y / 10000f;
            shakeComponent.frequency = frequency / 10000f;
            shakeComponent.totalFrame = shakeFrame;
            shakeComponent.curFrame = shakeFrame;
            shakeComponent.unitId = unit.InstanceId;
            shakeComponent.token = new ETCancellationToken();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}