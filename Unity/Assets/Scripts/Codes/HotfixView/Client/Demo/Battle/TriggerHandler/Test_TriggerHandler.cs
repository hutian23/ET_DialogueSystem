namespace ET.Client
{
    public class Test_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Test";
        }

        //Test: xxx
        public override bool Check(BBParser parser, BBScriptData data)
        {
            return true;
        }
    }
}