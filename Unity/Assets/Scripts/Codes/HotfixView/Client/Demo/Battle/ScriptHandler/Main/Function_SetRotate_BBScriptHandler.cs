using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SetRotate_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetRotate";
        }

        //SetRotate: 
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SetRotate: (?<rotate>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["rotate"].Value, out long rotate))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            body.SetRotation(body.GetFlip() * Mathf.Deg2Rad * (rotate / 10000f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}