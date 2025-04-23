using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SetTransition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetTransition";
        }

        //SetTransition: TransitionFlag, 开启 / 关闭;
        //SetTransition: NoPreSquat, true / false;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            
            Match match = Regex.Match(data.opLine, @"SetTransition: (?<transition>\w+), (?<enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            Transition transition = unit.GetComponent<Transition>();

            switch (match.Groups["enable"].Value)
            {
                case "false":
                    transition.RemoveFlag(match.Groups["transition"].Value);
                    return Status.Success;
                case "true":
                    transition.AddFlag(match.Groups["transition"].Value);
                    return Status.Success;
                default:
                    Log.Error($"match failed: {data.opLine}");
                    return Status.Failed;
            }
        }
    }
}