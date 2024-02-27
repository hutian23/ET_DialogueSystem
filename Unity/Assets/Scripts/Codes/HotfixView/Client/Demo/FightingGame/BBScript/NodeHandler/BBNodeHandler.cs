namespace ET.Client
{
    public class BBNodeHandler: NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            
            dialogueComponent.GetComponent<GatlingCancel>().Clear(); // 移除所有加特林取消
            
            dialogueComponent.GetComponent<BBParser>().InitScript(node.BBScript);
            await dialogueComponent.GetComponent<BBParser>().Main(token);
            
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}