using System;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(ScreenShakeComponent))]
    public class Function_ScreenShake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ScreenShake";
        }

        //ScreenShakeX: 1000, 1000, 15000, 15, 1; (ShakeLength_X, ShakeLength_Y, Frequency, ShakeFrame, ShakeMode)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ScreenShake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?), (?<ShakeMode>.*?);");
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

            VirtualCameraManager.Instance.RemoveComponent<ScreenShakeComponent>();
            ScreenShakeComponent screenShake = VirtualCameraManager.Instance.AddComponent<ScreenShakeComponent>(true);
            screenShake.shakeLength_X = shakeLength_X / 10000f;
            screenShake.shakeLength_Y = shakeLength_Y / 10000f;
            screenShake.frequency = frequency / 10000f;
            screenShake.totalFrame = shakeFrame;
            screenShake.curFrame = shakeFrame;
            screenShake.shakeMode = shakeMode;
            screenShake.activeCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}