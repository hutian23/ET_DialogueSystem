using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public class MarkerEvent_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MarkerEvent";
        }

        //MarkerEvent: (Mai_GroundDash_GC);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MarkerEvent: \((.*?)\)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //即用即插
            if (parser.GetComponent<MarkerEventParser>() == null)
            {
                parser.AddComponent<MarkerEventParser>();
                token.Add(parser.RemoveComponent<MarkerEventParser>);
            }

            //缓存动画帧事件的起始指针
            int index = parser.function_Pointers[data.functionID];
            parser.GetComponent<MarkerEventParser>().RegistMarker(match.Groups[1].Value, index);

            //跳过动画帧事件的代码块
            for (int i = index; i < parser.opDict.Count; i++)
            {
                if (parser.opDict[i].Equals("EndMarkerEvent:"))
                {
                    break;
                }

                parser.function_Pointers[data.functionID]++;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}