using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GroundCollisionComponent))]
    public class Function_EnableGroundCollisionCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGroundCollisionCheck";
        }

        //EnableGroundCollisionCallback: Active;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGroundCollisionCheck: (?<Active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.RemoveComponent<GroundCollisionComponent>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            parser.AddComponent<GroundCollisionComponent>(true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}