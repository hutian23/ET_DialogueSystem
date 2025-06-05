namespace ET.Client
{
    public class Condition_IsTCOption_BBScriptHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "IsTCOption";
        }

        // 只有在TargetCancelComponent挂载时，才会进入该行为
        public override bool Check(BBParser parser, BBScriptData data)
        {
            return false;
        }
    }
}