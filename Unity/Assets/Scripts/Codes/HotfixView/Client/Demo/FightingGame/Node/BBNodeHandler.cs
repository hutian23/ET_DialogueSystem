namespace ET.Client
{
    public class BBNodeHandler: NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            BBParser bbParser = dialogueComponent.GetComponent<BBParser>();

            // bbParser.Init(node.BBScript);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}