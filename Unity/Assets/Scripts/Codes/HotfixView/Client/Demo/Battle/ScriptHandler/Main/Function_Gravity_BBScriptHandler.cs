using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GravityCheckAbility))]
    public class Function_Gravity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Gravity";
        }

        //Gravity: 0;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"Gravity: (?<gravity>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["gravity"].Value, out long gravity))
            {
                Log.Error($"cannot format {match.Groups["gravity"].Value} to long!!!");
            }

            //2. 设置重力
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            GravityCheckAbility gc = buffManager.GetComponent<GravityCheckAbility>();
            if (gc == null)
            {
                Log.Error($"does not exist buff GravityCheckComponent!");
                return Status.Failed;
            }

            //限制重力值
            float g = gravity / 1000f;
            if (g > gc.maxGravity)
            {
                Log.Error("gravity is greater than max gravity!");
                g = gc.maxGravity;
            }
            gc.gravity = g;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}