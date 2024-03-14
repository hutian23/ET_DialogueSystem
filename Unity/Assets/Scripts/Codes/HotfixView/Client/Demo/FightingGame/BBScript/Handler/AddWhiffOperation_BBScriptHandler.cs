using System.Text.RegularExpressions;

namespace ET.Client
{
    public class AddWhiffOperation_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddWhiffOperation";
        }

        //AddWhiffOperation: 'Rg_5C';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine,"AddWhiffOperation: '(?<whiff>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string skillValue = match.Groups["whiff"].Value;
            parser.GetParent<DialogueComponent>().GetComponent<WhiffCancel>().AddTag(skillValue);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}