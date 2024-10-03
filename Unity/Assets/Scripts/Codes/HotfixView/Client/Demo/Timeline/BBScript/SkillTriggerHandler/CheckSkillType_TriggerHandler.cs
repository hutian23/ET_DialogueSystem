namespace ET.Client
{
    public class CheckSkillType_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "MoveType";
        }

        //SkillType: special;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            
            return false;
        }
    }
}