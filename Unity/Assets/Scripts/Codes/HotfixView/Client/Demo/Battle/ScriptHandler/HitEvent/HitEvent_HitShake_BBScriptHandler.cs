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

        //HitShake: 1000, 1000, 15000, 15; (ShakeLength_X, ShakeLength_Y, Frequency, ShakeFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitShake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
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
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength_X"].Value} / {match.Groups["ShakeLength_Y"].Value} /{match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            CollisionInfo info = parser.GetComponent<HitComponent>().GetInfo();
            
            b2Body _body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit _unit = Root.Instance.Get(_body.unitId) as Unit;
            BBParser _parser = _unit.GetComponent<BBParser>();

            _parser.RemoveComponent<ShakeComponent>();
            
            
            ShakeComponent shakeComponent = _unit.AddComponent<ShakeComponent>();
            shakeComponent.shakeLength_X = shakeLength_X / 10000f;
            shakeComponent.shakeLength_Y = shakeLength_Y / 10000f;
            shakeComponent.frequency = frequency / 10000f;
            shakeComponent.curFrame = shakeFrame;
            shakeComponent.totalFrame = shakeFrame;
            shakeComponent.unitId = _unit.InstanceId;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}