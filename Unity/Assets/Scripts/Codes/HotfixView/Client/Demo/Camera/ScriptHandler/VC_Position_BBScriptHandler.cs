using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class VC_Position_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Position";
        }

        //VC_Position: 0, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Position: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }

            Camera.main.transform.position = new Vector3(posX / 10000f, posY / 10000f, -10);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}