using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class InputBuffer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InputBuffer";
        }

        //InputBuffer: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"InputBuffer: ((?<InputBuffer>\w+));");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            
            inputWait.BufferFlag = match.Groups["InputBuffer"].Value.Equals("true");
            token.Add(() =>
            {
                inputWait.BufferFlag = false;
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}