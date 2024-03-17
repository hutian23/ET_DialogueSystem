using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveGatlingCancel_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveGatlingCancel";
        }

        //RemoveGatlingCancel: 'Sol_5s';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "RemoveGatlingCancel: '(?<skill>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string skillValue = match.Groups["skill"].Value;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}