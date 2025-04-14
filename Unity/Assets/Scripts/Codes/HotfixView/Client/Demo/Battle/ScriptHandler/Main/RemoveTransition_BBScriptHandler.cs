using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveTransition_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveTransition";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "RemoveTransition: '(?<transition>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string transition = $"Transition_{match.Groups["transition"].Value}";
            parser.GetParent<Unit>().GetComponent<BehaviorMachine>().TryRemoveTmpParam(transition);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}