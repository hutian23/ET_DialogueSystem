namespace ET.Client
{
    public class NodeCheckerAttribute: BaseAttribute
    {
    }
    
    [NodeChecker]
    public abstract class NodeCheckerHandler
    {
        public abstract int Check(Unit unit, NodeChecker nodeChecker);
    }
}