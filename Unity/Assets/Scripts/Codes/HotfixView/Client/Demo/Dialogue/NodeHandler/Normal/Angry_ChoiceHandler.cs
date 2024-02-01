namespace ET.Client
{
    public class Angry_ChoiceHandler: NodeHandler<Angry_ChoiceNode>
    {
        protected override async ETTask<Status> Run(Unit unit, Angry_ChoiceNode node, ETCancellationToken token)
        {
            Log.Warning($"Angry_Choice {node.TargetID}");
            await TimerComponent.Instance.WaitAsync(1000, token);
            if (token.IsCancel()) return Status.Failed;

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            node.Angrys.ForEach(targetID => dialogueComponent.PushNextNode(targetID));
            node.Normal.ForEach(targetID => dialogueComponent.PushNextNode(targetID));
            return Status.Success;
        }
    }
}