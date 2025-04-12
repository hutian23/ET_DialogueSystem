using System.Text.RegularExpressions;

namespace ET.Client
{
    public class EnableWhiffCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableWhiffCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableWhiffCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1.初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            if (parser.ContainParam("WhiffCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("WhiffCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("WhiffCancel_Timer");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffCancelTimer, unit);
            HashSetComponent<string> hashSetComponent = HashSetComponent<string>.Create();
            parser.RegistParam("WhiffCancel_Timer", timer);
            parser.RegistParam("WhiffCancel_Options", hashSetComponent);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
                hashSetComponent?.Dispose();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}