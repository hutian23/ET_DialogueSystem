namespace ET.Client
{
    public class CheckFlag_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CheckFlag";
        }

        //CheckFlag: "RunToIdle";
        public override bool Check(BBParser parser, BBScriptData data)
        {
            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            return buffer.ContainFlag("RunToIdle");
        }
    }
}