using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class VC_MoveTo_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_MoveTo";
        }

        //VC_MoveTo: 150000, -100000, 30000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_MoveTo: (?<CenterX>.*?), (?<CenterY>.*?), (?<Damping>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["Damping"].Value, out long damping))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} / {match.Groups["CenterY"].Value} / {match.Groups["Damping"].Value} to long!");
                return Status.Failed;
            }

            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            
            Vector2 targetPos = new Vector2(centerX, centerY) / 10000f;
            
            while (Vector2.Distance(targetPos, Camera.main.transform.position) > 0.1f)
            {
                Camera.main.transform.position = (Vector3)Vector2.Lerp(Camera.main.transform.position, targetPos, damping / 600000f) + new Vector3(0, 0, -10f);
                await lateUpdateTimer.WaitFrameAsync(token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}