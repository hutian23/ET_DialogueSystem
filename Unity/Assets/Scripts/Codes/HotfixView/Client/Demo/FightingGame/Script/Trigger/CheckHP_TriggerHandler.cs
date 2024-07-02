namespace ET.Client.Trigger
{
    public class CheckHP_TriggerHandler: TriggerHandler
    {
        public override string GetTriggerType()
        {
            return "HP";
        }

        public override bool Check(ScriptParser parser, ScriptData data)
        {
            Log.Warning("Check HP");
            return true;
        }
    }
}