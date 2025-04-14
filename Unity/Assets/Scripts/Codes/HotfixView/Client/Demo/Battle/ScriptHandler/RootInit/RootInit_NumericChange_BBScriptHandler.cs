using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(NumericCallback))]
    [FriendOf(typeof(BBNumeric))]
    public class RootInit_NumericChange_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericChange";
        }

        //NumericChange: DashCount
        //EndNumericCallback:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配
            Match match = Regex.Match(data.opLine, @"NumericChange: (?<NumericType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //2. 跳过Move代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndNumericChange:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;

            //3. 数值更新回调
            Unit unit = parser.GetParent<Unit>();
            BBNumeric bbNumeric = unit.GetComponent<BBNumeric>();
            NumericCallback callback = bbNumeric.AddChild<NumericCallback>();
            callback.startIndex = startIndex;
            callback.endIndex = endIndex;
            callback.NumericType = match.Groups["NumericType"].Value;
            bbNumeric.NumericCallbackDict.Add(match.Groups["NumericType"].Value, callback.InstanceId);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}