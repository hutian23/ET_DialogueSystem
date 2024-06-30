namespace ET.Client
{
    public class BBScriptCheckAttribute: BaseAttribute
    {
    }

    [BBScriptCheck]
    public abstract class BBCheckHandler
    {
        public abstract string GetBehaviorType();

        public abstract ETTask<Status> Handle(Unit unit, ETCancellationToken token);
    }
}