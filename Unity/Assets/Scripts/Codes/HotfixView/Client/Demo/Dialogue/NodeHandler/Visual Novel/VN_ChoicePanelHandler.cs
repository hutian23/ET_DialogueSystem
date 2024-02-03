using System.Collections.Generic;

namespace ET.Client
{
    public class VN_ChoicePanelHandler: NodeHandler<VN_ChoicePanel>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ChoicePanel node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            var nodelist = new List<VN_ChoiceNode>();
            node.children.ForEach(targetID =>
            {
                if (dialogueComponent.GetNode(targetID) is not VN_ChoiceNode choiceNode) return;
                int ret = DialogueDispatcherComponent.Instance.Checks(unit, choiceNode.checkList);
                if (ret != 0) return;
                
                dialogueComponent.SetNodeStatus(choiceNode,Status.Choice);
                nodelist.Add(choiceNode);
            });
            //刷新UI
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.ShowChoicePanel(nodelist);

            //这里这个阻塞的意义是什么? 如果workQueue为空，就判定为整个携程执行完毕。
            WaitChoiceNode wait = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitChoiceNode>(token);
           
            dlgDialogue.HideChoicePanel(); //关闭选项版
            nodelist.ForEach(choice => dialogueComponent.SetNodeStatus(choice, Status.None)); //刷新视图状态
            
            if (token.IsCancel()) return Status.Failed;
            dialogueComponent.PushNextNode(wait.next);
            
            return Status.Success;
        }
    }
}