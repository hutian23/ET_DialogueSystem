using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetDrive_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetDrive";
        }

        //SetDrive: 'DashDrive';
        //派生  dash ---> dashAttack
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "SetDrive: '(?<drive>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.GetParent<TimelineComponent>()
                    .GetComponent<SkillBuffer>()
                    .RegistParam($"Drive_{match.Groups["drive"].Value}", true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}