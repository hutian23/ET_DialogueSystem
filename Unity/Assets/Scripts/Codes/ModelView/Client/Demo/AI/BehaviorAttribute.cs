namespace ET.Client
{
    public class BehaviorAttribute: BaseAttribute
    {
    }

    [Behavior]
    public abstract class BehaviorHandler
    {
        public abstract int Check(Unit unit, BehaviorConfig config);

        public abstract ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token);
    }
}