using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(EnemyFlipCheckComponent))]
    public class Function_EnableEnemyFlipCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableEnemyFlipCheck";
        }

        //EnableEnemyFlipCheck: Active;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableEnemyFlipCheck: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.RemoveComponent<EnemyFlipCheckComponent>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            parser.AddComponent<EnemyFlipCheckComponent>(true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}