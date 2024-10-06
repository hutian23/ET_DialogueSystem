using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveFlag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveFlag";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RemoveFlag: '(?<flag>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.RemoveFlag(match.Groups["flag"].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}