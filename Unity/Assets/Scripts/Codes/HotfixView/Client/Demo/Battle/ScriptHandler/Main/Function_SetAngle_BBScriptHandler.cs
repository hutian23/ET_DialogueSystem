using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SetAngle_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetAngle";
        }

        //SetRotate: 
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SetAngle: (?<angle>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["angle"].Value, out long angle))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            body.SetAngle(body.GetFlip() * angle / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}