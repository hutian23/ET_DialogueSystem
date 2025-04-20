namespace ET.Client
{
    public class Condition_CanAirDash_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CanAirDash";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            return false;
        }
    }
}