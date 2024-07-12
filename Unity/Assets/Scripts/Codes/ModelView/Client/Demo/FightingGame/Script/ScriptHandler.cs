namespace ET.Client
{
    public class ScriptAttribute: BaseAttribute
    {
    }

    [Script]
    public abstract class ScriptHandler
    {
        public abstract string GetOpType();

        public abstract ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token);
    }
} 