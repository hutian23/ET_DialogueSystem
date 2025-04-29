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
            AirCheckAbility ability = parser.GetParent<Unit>().GetComponent<BuffManager>().GetComponent<AirCheckAbility>();
            Log.Warning(ability.ToJson());
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}