using Box2DSharp.Dynamics;
using MongoDB.Bson;

namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.b2Body))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            Log.Warning("Exit");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}