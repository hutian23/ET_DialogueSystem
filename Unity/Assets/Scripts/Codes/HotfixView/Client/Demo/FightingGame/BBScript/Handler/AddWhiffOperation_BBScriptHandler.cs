using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBufferComponent))]
    public class AddWhiffOperation_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddWhiffOperation";
        }

        //AddWhiffOperation: 'Rg_5C';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AddWhiffOperation: '(?<whiff>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string whiffTag = match.Groups["whiff"].Value;
            BehaviorBufferComponent bufferComponent = parser.GetParent<BehaviorBufferComponent>();
            long order = bufferComponent.GetOrder(whiffTag);
            bufferComponent.WhiffSet.Add(order);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}