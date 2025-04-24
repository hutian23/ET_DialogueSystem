using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Thrones_SubState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_SubState";
        }

        //Thrones_SubState: 1, Throne_WallThrow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Thrones_SubState: (?<No>.*?), (?<SubState>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            //
            // long instanceId = parser.GetParam<long>($"Throne_{match.Groups["No"].Value}");
            // TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
            // BehaviorMachine machine = timelineComponent.GetParent<Unit>().GetComponent<BehaviorMachine>();
            //
            // if (machine.ContainParam("DeadFlag"))
            // {
            //     return Status.Success;
            // }
            // machine.Reload(match.Groups["SubState"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}