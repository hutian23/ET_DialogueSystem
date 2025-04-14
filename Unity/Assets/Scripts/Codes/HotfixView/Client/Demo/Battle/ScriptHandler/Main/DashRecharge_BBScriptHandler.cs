using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BBNumeric))]
    public class DashRecharge_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DashRecharge";
        }

        //DashRecharge: 2, 200;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"DashRecharge: (?<Count>.*?), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["WaitFrame"].Value, out long waitFrame) || !int.TryParse(match.Groups["Count"].Value, out int count))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} or {match.Groups["Count"].Value} to long!!");
                return Status.Failed;
            }

            WaitCor(parser, waitFrame, count).Coroutine();

            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask<Status> WaitCor(BBParser parser, long waitFrame, int count)
        {
            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BBNumeric numeric = unit.GetComponent<BBNumeric>();

            if (!numeric.NumericDict.ContainsKey("DashCount"))
            {
                Log.Error($"must add DashCount to BBNumericComponent!!");
                return Status.Failed;
            }
            
            //1. 初始化
            if (machine.ContainParam("DashRecharge_Token"))
            {
                ETCancellationToken _token = machine.GetParam<ETCancellationToken>("DashRecharge_Token");
                _token.Cancel();
            }
            machine.TryRemoveParam("DashRecharge_Token");
            
            //2. 注册变量
            ETCancellationToken token = new();
            machine.RegistParam("DashRecharge_Token", token);
            machine.Token.Add(() =>
            {
                token.Cancel();
            });

            await BBTimerManager.Instance.SceneTimer().WaitAsync(waitFrame, token);
            if (token.IsCancel()) return Status.Failed;
            
            //3. 初始化
            if (machine.ContainParam("DashRecharge_Token"))
            {
                ETCancellationToken _token = machine.GetParam<ETCancellationToken>("DashRecharge_Token");
                _token.Cancel();
            }
            machine.TryRemoveParam("DashRecharge_Token");
            
            //4. dash充能完毕
            numeric.Set("DashCount", count, true);
            return Status.Success;
        }
    }
}