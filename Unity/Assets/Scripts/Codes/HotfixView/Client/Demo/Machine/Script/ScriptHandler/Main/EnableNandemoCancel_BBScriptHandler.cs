using System.Text.RegularExpressions;

namespace ET.Client
{
    //EnableNandemoCancel: true;
    public class EnableNandemoCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableNandemoCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableNandemoCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
           
            if (parser.ContainParam("NandemoCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("NandemoCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("NandemoCancel_Timer");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.NandemoCancelTimer, unit);
            parser.RegistParam("NandemoCancel_Timer", timer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}