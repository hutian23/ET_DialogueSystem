using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(ShakeComponent))]
    public class Function_Shake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Shake";
        }

        //Shake: ShakeLengthX, ShakeLengthY, Frequency, ShakeFrame, ShakeMode;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?), (?<ShakeMode>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency) ||
                !Enum.TryParse(match.Groups["ShakeMode"].Value, out ShakeMode mode))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();
            
            unit.RemoveComponent<ShakeComponent>();
            ShakeComponent shakeComponent = unit.AddComponent<ShakeComponent>();
            shakeComponent.shakeLength = new Vector2(shakeLength_X, shakeLength_Y) / 10000f;
            shakeComponent.frequency = frequency / 10000f;
            shakeComponent.curFrame = shakeFrame;
            shakeComponent.totalFrame = shakeFrame;
            shakeComponent.unitId = unit.InstanceId;
            shakeComponent.shakeMode = mode;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}