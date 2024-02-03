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
                nodelist.Add(choiceNode);
            });
            //刷新UI
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.ShowChoicePanel(nodelist);

            //这里这个阻塞的意义是什么? 如果workQueue为空，就判定为整个携程执行完毕。
            WaitChoiceNode wait = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitChoiceNode>(token);
            if (token.IsCancel()) return Status.Failed;
            dialogueComponent.PushNextNode(wait.next);
            //关闭选项版
            dlgDialogue.HideChoicePanel();
            
            return Status.Success;
        }
    }
}