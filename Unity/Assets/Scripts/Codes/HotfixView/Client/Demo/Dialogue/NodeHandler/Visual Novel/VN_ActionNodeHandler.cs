namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            //1. 执行脚本
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, token);
            if (token.IsCancel()) return Status.Failed;

            //2. 打印携程
            DialogueHelper.ReplaceModel(unit, ref node.text);
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            await dialogueComponent.TypeCor(dlgDialogue.View.E_TextText, node.text, token);
            if (token.IsCancel()) return Status.Failed;

            //3. 等待下一个节点入队
            dialogueComponent.WaitNextCor(token).Coroutine();
            await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitNextNode>(token);
            if (token.IsCancel()) return Status.Failed;
            dlgDialogue.RefreshArrow();
            
            dialogueComponent.PushNextNode(dialogueComponent.GetFirstNode(node.children));

            return Status.Success;
        }
    }
}