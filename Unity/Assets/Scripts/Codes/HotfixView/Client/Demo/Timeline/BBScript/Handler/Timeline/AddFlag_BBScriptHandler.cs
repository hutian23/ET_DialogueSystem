using System.Text.RegularExpressions;

namespace ET.Client
{
    public class AddFlag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddFlag";
        }

        //AddFlag: 'Run';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AddFlag: '(?<Flag>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.AddFlag(match.Groups["Flag"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}