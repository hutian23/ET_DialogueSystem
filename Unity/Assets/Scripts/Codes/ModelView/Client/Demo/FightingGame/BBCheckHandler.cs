namespace ET.Client
{
    public class BBScriptCheckAttribute: BaseAttribute
    {
    }

    [BBScriptCheck]
    public abstract class BBCheckHandler
    {
        public abstract string GetSkillType();

        public abstract ETTask Handle(Unit unit, ETCancellationToken token);
    }
}