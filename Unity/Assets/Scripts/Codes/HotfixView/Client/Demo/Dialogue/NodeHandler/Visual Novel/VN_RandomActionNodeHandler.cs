using System;

namespace ET.Client
{
    public class VN_RandomActionNodeHandler: NodeHandler<VN_RandomActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_RandomActionNode node, ETCancellationToken token)
        {
            int randomValue = new Random().Next(node.MinValue, node.MaxValue);
            var script = DialogueHelper.Replace(node.Script, "RandomValue", randomValue.ToString());
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, script, token);
            if (token.IsCancel()) return Status.Failed;

            var text = DialogueHelper.Replace(node.text, "RandomValue", randomValue.ToString());
            Log.Debug(text);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}