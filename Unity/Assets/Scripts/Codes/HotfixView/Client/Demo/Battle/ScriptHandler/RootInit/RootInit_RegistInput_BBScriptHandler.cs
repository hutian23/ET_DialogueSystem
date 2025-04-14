using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public class RootInit_RegistInput_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistInput";
        }

        //RegistInput: '236P';
        //note: 只能在RootInit携程中使用!
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistInput: (?<InputType>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //启动输入检测携程
            InputWait inputWait = parser.GetParent<Unit>().GetComponent<InputWait>();
            inputWait.handleQueue.Enqueue(match.Groups["InputType"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}