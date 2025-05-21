using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_GroundCollision_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "GroundCollision";
        }

        // GroundCollision: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"GroundCollision: (?<Active>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            GroundCollisionComponent collision = parser.GetComponent<GroundCollisionComponent>();
            switch (match.Groups["Active"].Value)
            {
                case "true":
                    return collision.GetGroundCollision();
                case "false":
                    return !collision.GetGroundCollision();
                default:
                    Log.Error($"matched failed");
                    return false;
            }
        }
    }
}