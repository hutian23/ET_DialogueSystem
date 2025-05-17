using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletFlip";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletFlip: (?<Flip>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            
            Unit unitA = parser.GetParent<Unit>();
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);

            int flip = bodyA.GetFlip();
            bodyB.SetFlip((FlipState)(-flip));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}