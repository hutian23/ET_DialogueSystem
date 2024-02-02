namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, token);

            if (token.IsCancel()) return Status.Failed;

            string replaceText = node.text;
            DialogueHelper.ReplaceModel(unit, ref replaceText);
            Log.Warning(replaceText);

            return Status.Success;
        }
    }
}