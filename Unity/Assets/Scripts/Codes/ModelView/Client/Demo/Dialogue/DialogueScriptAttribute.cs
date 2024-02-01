namespace ET.Client
{
    public class DialogueScriptAttribute: BaseAttribute
    {
    }
    
    [DialogueScript]
    public abstract class ScriptHandler
    {
        public abstract string GetOPType();
        public abstract ETTask Handle(Unit unit, string line, ETCancellationToken token);
    }
}