using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_Damage_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Damage";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Damage: (?<Damage>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["Damage"].Value, out int damage))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyB = boxB.GetParent<b2Body>();
            Unit unitB = Root.Instance.Get(bodyB.unitId) as Unit;

            HPAbility ability = unitB.GetComponent<BuffManager>().GetComponent<HPAbility>();
            ability.HPAdd(-damage);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}