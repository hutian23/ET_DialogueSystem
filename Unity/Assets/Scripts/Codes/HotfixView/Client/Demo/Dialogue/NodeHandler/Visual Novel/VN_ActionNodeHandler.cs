namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, token);
            if (token.IsCancel()) return Status.Failed;

            DialogueHelper.ReplaceModel(unit, ref node.text);
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            await dialogueComponent.TypeCor(dlgDialogue.View.E_TextText, node.text, token); //打印
            if (token.IsCancel()) return Status.Failed;

            await DialogueHelper.WaitNextCor(token);
            if (token.IsCancel()) return Status.Failed;
            
            foreach (var targetID in node.children)
            {
                DialogueNode child = dialogueComponent.GetNode(targetID);
                // 找到子节点中第一个符合条件的执行
                if (!child.NeedCheck || DialogueDispatcherComponent.Instance.Checks(unit, child.checkList) == 0)
                {
                    dialogueComponent.PushNextNode(targetID);
                    break;
                }
            }

            return Status.Success;
        }
    }
}