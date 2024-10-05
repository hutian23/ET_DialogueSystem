using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBufferComponent))]
    [FriendOf(typeof(InputCheck))]
    [FriendOf(typeof(InputWait))]
    public class RegistInput_BBScriptHandler : BBScriptHandler
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
            // BehaviorBufferComponent bufferComponent = parser.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
            // //存在相同组件
            // if (bufferComponent.inputCheckDict.ContainsKey(data.targetID))
            // {
            //     Log.Error($"already contain inputCheck: {data.targetID}");
            //     return Status.Failed;
            // }
            //
            // InputCheck inputCheck = bufferComponent.AddChild<InputCheck>();
            // bufferComponent.inputCheckDict.Add(data.targetID, inputCheck);
            // //初始化
            // inputCheck.targetID = data.targetID;
            // inputCheck.inputChecker = match.Groups["Checker"].Value;
            // if (!long.TryParse(match.Groups["LastedFrame"].Value, out long lastedFrame))
            // {
            //     Log.Error($"cannot convert {match.Groups["LastedFrame"]} to int !!!");
            //     return Status.Failed;
            // }
            // inputCheck.LastedFrame = lastedFrame;

            Match match = Regex.Match(data.opLine, @"RegistInput: (?<InputType>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.InputHandlers.Add(match.Groups["InputType"].Value);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}