using System;
using System.Numerics;
using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(ShakeComponent))]
    public class HitEvent_HitShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitShake";
        }

        //HitShake: 1000, 1000, 15000, 15, 1; (ShakeLength_X, ShakeLength_Y, Frequency, ShakeFrame, ShakeMode)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitShake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?), (?<ShakeMode>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency) ||
                !Enum.TryParse(match.Groups["ShakeMode"].Value, out ShakeMode shakeMode))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;

            unitB.RemoveComponent<ShakeComponent>();
            
            ShakeComponent shakeComponent = unitB.AddComponent<ShakeComponent>(true);
            shakeComponent.shakeLength = new Vector2(shakeLength_X, shakeLength_Y) / 10000f;
            shakeComponent.frequency = frequency / 10000f;
            shakeComponent.curFrame = shakeFrame;
            shakeComponent.totalFrame = shakeFrame;
            shakeComponent.unitId = unitB.InstanceId;
            shakeComponent.shakeMode = shakeMode;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}