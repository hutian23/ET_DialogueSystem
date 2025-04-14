using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetVelocityX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityX";
        }

        //SetVelocityX: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocityX: (?<Velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Velocity"].Value, out long velX))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long !!!");
                return Status.Failed;
            }
            
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            b2Unit.SetVelocityX(velX / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}