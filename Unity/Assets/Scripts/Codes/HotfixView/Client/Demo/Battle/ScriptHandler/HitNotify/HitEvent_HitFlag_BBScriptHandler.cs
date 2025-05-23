using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_HitFlag_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitFlag";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //HitParam: StopFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitParam: (?<Flag>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 查询组件
            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;
            BBParser parserB = unitB.GetComponent<BBParser>();
            
            //2. 添加flag
            parserB.RegistParam($"Flag_{match.Groups["Flag"].Value}", true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}