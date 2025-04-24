using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ThronesTest_DeadFlag_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThronesTest_DeadFlag";
        }

        //Thrones_DeadFlag: 1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ThronesTest_DeadFlag: (?<No>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            long instanceId = parser.GetParam<long>($"Throne_{match.Groups["No"].Value}");
            TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
            BehaviorMachine machine = timelineComponent.GetParent<Unit>().GetComponent<BehaviorMachine>();
            //
            // machine.TryRemoveParam("DeadFlag");
            // machine.RegistParam("DeadFlag", true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}