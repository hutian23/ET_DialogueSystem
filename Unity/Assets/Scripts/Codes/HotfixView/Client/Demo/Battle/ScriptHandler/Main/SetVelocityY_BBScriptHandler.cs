using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetVelocityY_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityY";
        }

        //SetVelocityY: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocityY: (?<Velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Velocity"].Value, out long velocity))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long");
                return Status.Failed;
            }
            
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            b2Unit.SetVelocityY(velocity / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}