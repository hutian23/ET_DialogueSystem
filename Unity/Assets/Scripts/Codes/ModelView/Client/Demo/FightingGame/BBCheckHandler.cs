namespace ET.Client
{
    public class BBScriptAttribute: BaseAttribute
    {
    }

    [BBScript]
    public abstract class BBCheckHandler
    {
        public abstract string GetSkillType();

        public abstract ETTask Handle(Unit unit, ETCancellationToken token);
    }
}