namespace ET.Client
{
    public class BBScriptAttribute: BaseAttribute
    {
    }

    [BBScript]
    public abstract class BBScriptHandler
    {
        public abstract string GetOPType();

        public abstract ETTask<Status> Handle(BBParser parser, BBScriptData data , ETCancellationToken token);
    }
}