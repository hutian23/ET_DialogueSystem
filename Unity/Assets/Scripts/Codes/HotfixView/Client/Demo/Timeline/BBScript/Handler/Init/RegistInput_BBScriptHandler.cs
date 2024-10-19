using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (InputCheck))]
    [FriendOf(typeof (InputWait))]
    public class RegistInput_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInput";
        }

        //RegistInput: '236P',5;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistInput: (?<InputType>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            BBInputHandler handler = DialogueDispatcherComponent.Instance.GetInputHandler(match.Groups["InputType"].Value);
            inputWait.handlers.Add(handler);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}