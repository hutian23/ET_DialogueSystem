using System;

namespace ET.Client
{
    public class VN_RandomActionNodeHandler: NodeHandler<VN_RandomActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_RandomActionNode node, ETCancellationToken token)
        {
            int randomValue = new Random().Next(node.MinValue, node.MaxValue);
            return Status.Success;
        }
    }
}