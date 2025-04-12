using System.Text.RegularExpressions;

namespace ET.Client
{
    // 加特林取消，当前动作只能被比自己层级高 or 添加了取消标签的动作取消
    public class EnableGatlingCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGatlingCancel";
        }

        //EnableGatlingCancel: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGatlingCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            if (parser.ContainParam("GatlingCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("GatlingCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("GatlingCancel_Timer");
            }
            if (parser.ContainParam("GatlingCancel_Options"))
            {
                HashSetComponent<string> GCOptions = parser.GetParam<HashSetComponent<string>>("GatlingCancel_Options");
                GCOptions.Dispose();
                parser.TryRemoveParam("GatlingCancel_Options");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.GatlingCancelTimer, unit);
            HashSetComponent<string> hashSetComponent = HashSetComponent<string>.Create();
            parser.RegistParam("GatlingCancel_Timer", timer);
            parser.RegistParam("GatlingCancel_Options", hashSetComponent);
            
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