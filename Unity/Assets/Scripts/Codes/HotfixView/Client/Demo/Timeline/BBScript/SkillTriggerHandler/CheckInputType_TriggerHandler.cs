namespace ET.Client
{
    public class CheckInputType_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InputType";
        }

        //InputType: 6;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            return BBInputComponent.Instance.ContainKey(BBOperaType.LEFT);
        }
    }
}