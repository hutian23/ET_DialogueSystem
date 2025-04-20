using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SetTransition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetTransition";
        }

        //SetTransition: NoPreSquat;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetTransition: (?<transition>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            Transition transition = unit.GetComponent<Transition>();
            transition.AddFlag(match.Groups["transition"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}