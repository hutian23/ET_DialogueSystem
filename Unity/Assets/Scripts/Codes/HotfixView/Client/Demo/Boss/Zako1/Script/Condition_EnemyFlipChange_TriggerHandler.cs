using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_EnemyFlipChange_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "EnemyFlipChange";
        }

        //EnemyFlipChange: false;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"EnemyFlipChange: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            Unit unitA = BBUnitHelper.GetPlayer(parser.ClientScene());
            Unit unitB = parser.GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);

            FlipState curFlip = bodyB.GetPosition().X >= bodyA.GetPosition().X ? FlipState.Left : FlipState.Right;
            bool flipChange = curFlip != (FlipState)bodyB.GetFlip();
            
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return flipChange;
                case "false":
                    return !flipChange;
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}