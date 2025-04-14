using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckReachX_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "ReachX";
        }

        //ReachX: 10500, Right
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"ReachX: (?<Position>.*?), (?<transition>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            if (!long.TryParse(match.Groups["Position"].Value, out long targetX))
            {
                Log.Error($"cannot format {match.Groups["Position"].Value} to long !!!");
                return false;
            }

            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            float curPosX = body.GetPosition().X;
            
            switch (match.Groups["transition"].Value)
            {
                case "Right":
                    return curPosX <= targetX / 10000f;
                case "Left":
                    return curPosX >= targetX / 10000f;
                default:
                    return false;
            }
        }
    }
}