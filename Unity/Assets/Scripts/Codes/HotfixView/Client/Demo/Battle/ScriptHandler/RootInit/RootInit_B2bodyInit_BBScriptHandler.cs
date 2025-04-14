using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [FriendOf(typeof(b2WorldManager))]
    public class RootInit_B2bodyInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "B2bodyInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.RemoveComponent<SceneBoxHandler>();
            parser.AddComponent<SceneBoxHandler>();

            //1. 不存在刚体，创建
            Unit unit = parser.GetParent<Unit>();
            if (!b2WorldManager.Instance.BodyDict.TryGetValue(unit.InstanceId, out long _))
            {
                b2WorldManager.Instance.CreateBody(unit.InstanceId, new BodyDef() { BodyType = BodyType.StaticBody, Position = Vector2.Zero });
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}