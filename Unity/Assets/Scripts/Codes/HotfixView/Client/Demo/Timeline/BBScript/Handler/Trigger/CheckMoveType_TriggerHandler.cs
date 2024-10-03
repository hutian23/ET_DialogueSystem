namespace ET.Client
{
    public class CheckMoveType_TriggerHandler : TriggerHandler
    {
        public override string GetTriggerType()
        {
            return "MoveType";
        }

        //MoveType: Move;
        public override bool Check(ScriptParser parser, ScriptData data)
        {
            return true;
        }
    }
}