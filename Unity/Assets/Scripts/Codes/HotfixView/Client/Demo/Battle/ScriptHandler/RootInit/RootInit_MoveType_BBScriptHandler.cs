using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public class RootInit_MoveType_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveType";
        }

        // MoveType: Normal;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MoveType: (?<MoveType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!Enum.TryParse(match.Groups["MoveType"].Value, out MoveType moveType))
            {
                Log.Error($"cannot parser {match.Groups["MoveType"].Value} to MoveType");
                return Status.Failed;
            }
            
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            info.moveType = moveType;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}