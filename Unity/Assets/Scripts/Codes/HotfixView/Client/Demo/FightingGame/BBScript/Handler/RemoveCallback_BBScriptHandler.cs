using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RemoveCallback_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveCallback";
        }

        //RemoveCallback type = OnBlock;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            ObjectWait objectWait = unit.GetComponent<DialogueComponent>().GetComponent<ObjectWait>();
            Match match = Regex.Match(opCode, @"RemoveCallback type = (?<Type>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }
            
            string Type = match.Groups["Type"].Value;
            switch (Type)
            {
                case "OnBlock":
                    objectWait.Notify(new WaitBlock(){Error = WaitTypeError.Destroy});
                    break;
                case "OnCounterHit":
                    objectWait.Notify(new WaitCounterHit(){Error = WaitTypeError.Destroy});
                    break;
                case "OnHit":
                    objectWait.Notify(new WaitHit(){Error = WaitTypeError.Destroy});
                    break;
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}