using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RegistCallback_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // WaitCallback(unit, opCode, token).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        //RegistCallback type = OnBlock func = OnBlock;
        // private async ETTask WaitCallback(Unit unit, string opCode, ETCancellationToken token)
        // {
        //     ObjectWait objectWait = unit.GetComponent<DialogueComponent>().GetComponent<ObjectWait>();
        //     BBParser bbParser = unit.GetComponent<DialogueComponent>().GetComponent<BBParser>();
        //
        //     Match match = Regex.Match(opCode, @"RegistCallback type = (?<Type>\w+) func = (?<Function>\w+);");
        //     if (!match.Success)
        //     {
        //         DialogueHelper.ScripMatchError(opCode);
        //         return;
        //     }
        //
        //     string callbackType = match.Groups["Type"].Value;
        //     switch (callbackType)
        //     {
        //         case "OnBlock":
        //             WaitBlock waitBlock = await objectWait.Wait<WaitBlock>(token);
        //             if (waitBlock.Error != WaitTypeError.Success) return;
        //             break;
        //         case "OnCounterHit":
        //             WaitCounterHit waitCounterHit = await objectWait.Wait<WaitCounterHit>(token);
        //             if (waitCounterHit.Error != WaitTypeError.Success) return;
        //             break;
        //         case "OnHit":
        //             WaitHit waitHit = await objectWait.Wait<WaitHit>(token);
        //             if (waitHit.Error != WaitTypeError.Success) return;
        //             break;
        //     }
        //     await bbParser.SubCoroutine(match.Groups["Function"].Value);
        // }
    }
}