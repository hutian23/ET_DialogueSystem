using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBInputComponent))]
    public class RegistInputChecker_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInputChecker";
        }

        //RegistInputChecker name = Sol_GunFlame;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"RegistInputChecker name = (?<Checker>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            BBInputComponent bbInput = unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>();
            bbInput.bbCheckers.Add($"{match.Groups["Checker"].Value}_CheckHandler");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}