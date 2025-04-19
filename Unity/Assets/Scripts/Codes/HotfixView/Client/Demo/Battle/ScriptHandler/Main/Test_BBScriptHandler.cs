using MongoDB.Bson;

namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BlackBoard blackBoard = parser.GetParent<Unit>().GetComponent<BlackBoard>();
            blackBoard.RegistValue("Test", 2);
            blackBoard.RegistValue("Test2", new LandCallback(){instanceId = 100001});

            LandCallback landCallback = blackBoard.GetValue<LandCallback>("Test2");
            Log.Warning(landCallback.ToJson());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}