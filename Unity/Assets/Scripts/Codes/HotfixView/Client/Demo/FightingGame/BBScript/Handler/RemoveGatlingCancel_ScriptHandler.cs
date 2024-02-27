using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveGatlingCancel_ScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveGatlingCancel";
        }

        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"RemoveGatlingCancel skill = (?<skill>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            string skillValue = match.Groups["skill"].Value;
            unit.GetComponent<DialogueComponent>().GetComponent<GatlingCancel>().RemoveTag(skillValue);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}