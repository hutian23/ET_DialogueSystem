using System.Text.RegularExpressions;

namespace ET.Client
{
    public class InitHP_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HP";
        }

        //HP: 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HP: (?<HP>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["HP"].Value, out int MaxHPBase);
            parser.GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<NumericComponent>()[NumericType.Hp] = MaxHPBase;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}