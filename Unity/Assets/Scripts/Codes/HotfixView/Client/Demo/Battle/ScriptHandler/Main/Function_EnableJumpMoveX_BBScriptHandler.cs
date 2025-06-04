using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableJumpMoveX_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableJumpMoveX";
        }

        // EnableJumpMoveX: MoveX, Active;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableJumpMoveX: (?<MoveX>.*?), (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error("Matched failed");
                return Status.Failed;
            }

            parser.RemoveComponent<JumpMoveXComponent>();
            if (match.Groups["Enable"].Value.Equals("true"))
            { 
                parser.AddComponent<JumpMoveXComponent, float>(moveX / 10000f, true);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}