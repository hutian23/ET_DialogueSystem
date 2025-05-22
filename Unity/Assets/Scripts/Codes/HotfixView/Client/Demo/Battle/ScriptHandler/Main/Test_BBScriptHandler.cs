using Box2DSharp.Dynamics;

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
            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            body.AddChild<b2Box,FixtureDef>(new FixtureDef());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}