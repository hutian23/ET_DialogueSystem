using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBInputComponent))]
    [FriendOf(typeof (BBParser))]
    // [FriendOf(typeof (BehaviorInfo))]
    public class RegistInputChecker_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInput";
        }

        //RegistInput: '236P',5;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Match match = Regex.Match(data.opLine, @"RegistInput: '(?<Checker>\w+)',(?<LastedFrame>\w+);");
            // if (!match.Success)
            // {
            //     DialogueHelper.ScripMatchError(data.opLine);
            //     return Status.Failed;
            // }
            //
            // BehaviorInfo skillInfo = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetSkillInfo(parser.currentID);
            // skillInfo.inputChecker = match.Groups["Checker"].Value;
            // int.TryParse(match.Groups["LastedFrame"].Value, out int lastedFrame);
            // skillInfo.LastedFrame = lastedFrame;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}