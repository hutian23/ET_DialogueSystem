using System;

namespace ET.Client
{
    public class VN_RandomActionNodeHandler: NodeHandler<VN_RandomActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_RandomActionNode node, ETCancellationToken token)
        {
            int randomValue;
            if (!node.UseSharedVariable)
            {
                randomValue = new Random().Next(node.MinValue, node.MaxValue + 1);
            }
            else
            {
                DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
                int min = dialogueComponent.GetVariable<int>(node.minVariable);
                int max = dialogueComponent.GetVariable<int>(node.maxVariable);
                randomValue = new Random().Next(min, max + 1);
            }

            DialogueHelper.ReplaceCustomModel(ref node.text, "Random", randomValue.ToString());
            DialogueHelper.ReplaceCustomModel(ref node.Script, "Random", randomValue.ToString());

            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, token);
            if (token.IsCancel()) return Status.Failed;

            Log.Warning(node.text);

            return Status.Success;
        }
    }
}