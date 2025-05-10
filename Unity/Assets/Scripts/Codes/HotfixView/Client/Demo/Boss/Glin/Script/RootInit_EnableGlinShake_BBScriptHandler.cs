using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(GlinShakeComponent))]
    public class RootInit_EnableGlinShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGlinShake";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGlinShake: (?<Active>\w+), (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeLength_X"].Value} / {match.Groups["ShakeLength_Y"].Value} /{match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            parser.RemoveComponent<GlinShakeComponent>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            GlinShakeComponent glinShake = parser.AddComponent<GlinShakeComponent>(true);
            glinShake.shakeLength_X = shakeLength_X / 10000f;
            glinShake.shakeLength_Y = shakeLength_Y / 10000f;
            glinShake.frequency = frequency / 10000f;
            glinShake.activeCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}