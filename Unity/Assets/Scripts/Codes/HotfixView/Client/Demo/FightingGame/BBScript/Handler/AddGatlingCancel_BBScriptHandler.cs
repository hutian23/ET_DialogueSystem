using System.Text.RegularExpressions;

namespace ET.Client
{
    public class AddGatlingCancel_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddGatlingCancel";
        }

        //AddGatlingCancel skill = Sol_5HS;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"AddGatlingCancel skill = (?<skill>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            string skillValue = match.Groups["skill"].Value;
            unit.GetComponent<DialogueComponent>().GetComponent<GatlingCancel>().AddTag(skillValue);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}