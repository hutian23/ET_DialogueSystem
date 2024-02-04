using System;

namespace ET.Client
{
    public class NodeCheckerAttribute: BaseAttribute
    {
    }
    
    public interface NodeCheckHandler
    {
        public int Check(Unit unit, object nodeCheckConfig);

        public Type GetNodeCheckType();
    }
    
    [NodeChecker]
    public abstract class NodeCheckHandler<NodeCheck> : NodeCheckHandler where NodeCheck : NodeCheckConfig
    {
        public int Check(Unit unit, object nodeCheckConfig)
        {
            if (nodeCheckConfig is not NodeCheck nodeCheck)
            {
                Log.Error($"nodeCheck类型转换错误: {nodeCheckConfig.GetType().FullName} to {typeof(NodeCheck)}");
                return 1;
            }
            return this.Run(unit, nodeCheck);
        }

        public Type GetNodeCheckType()
        {
            return typeof (NodeCheck);
        }

        protected abstract int Run(Unit unit, NodeCheck nodeCheck);
    }
}