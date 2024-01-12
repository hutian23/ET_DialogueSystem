namespace ET.Client
{
    public class DialogueScriptAttribute: BaseAttribute
    {
    }
    
    // Action节点 一定要写很多继承，
    [DialogueScript]
    public abstract class ScriptHandler
    {
        public abstract string GetOPType();
        public abstract ETTask Handle(Unit unit, string line, ETCancellationToken token);
    }
}