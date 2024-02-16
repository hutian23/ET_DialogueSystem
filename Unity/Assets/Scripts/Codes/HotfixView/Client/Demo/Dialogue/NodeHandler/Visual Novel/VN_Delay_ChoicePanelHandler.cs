using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(DialogueStorage))]
    public class VN_Delay_ChoicePanelHandler : NodeHandler<VN_Delay_ChoicePanel>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_Delay_ChoicePanel node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            //1. 正常展示
            var nodeList = new List<VN_ChoiceNode>();
            node.normal.ForEach(targetID =>
            {
                if (dialogueComponent.GetNode(targetID) is not VN_ChoiceNode choiceNode) return;
                if (choiceNode.NeedCheck)
                {
                    int ret = DialogueDispatcherComponent.Instance.Checks(unit, choiceNode.checkList);
                    if (ret != 0) return;
                }

                dialogueComponent.SetNodeStatus(choiceNode, Status.Choice);
                nodeList.Add(choiceNode);
            });
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.ShowChoicePanel(nodeList);

            ETCancellationToken delayToken = new(); //用于取消延时展示携程
            token.Add(delayToken.Cancel);

            async ETTask DelayCor()
            {
                await TimerComponent.Instance.WaitAsync(node.delayTime, delayToken);
                if (delayToken.IsCancel()) return;

                node.special.ForEach(targetID =>
                {
                    if (dialogueComponent.GetNode(targetID) is not VN_ChoiceNode choiceNode) return;
                    if (choiceNode.NeedCheck)
                    {
                        int ret = DialogueDispatcherComponent.Instance.Checks(unit, choiceNode.checkList);
                        if (ret != 0) return;
                    }

                    dialogueComponent.SetNodeStatus(choiceNode, Status.Choice);
                    nodeList.Add(choiceNode);
                });
                dlgDialogue.ShowChoicePanel(nodeList);
                //5. 选项节点存档要特殊处理一下
                dialogueComponent.AddTag(DialogueTag.CanEnterSetting);
                DialogueStorageManager.Instance.QuickSaveShot.currentID_Temp = node.GetID();
            }

            //2. 延时展示
            DelayCor().Coroutine();

            //3. 等待选择
            WaitChoiceNode wait = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitChoiceNode>(token);
            //4. 取消延时展示携程
            token.Remove(delayToken.Cancel);
            delayToken.Cancel();
            if (token.IsCancel()) return Status.Failed;

            dlgDialogue.HideChoicePanel(); //关闭选项版
            nodeList.ForEach(choice => dialogueComponent.SetNodeStatus(choice, Status.None)); //刷新视图状态
            dialogueComponent.PushNextNode(wait.next); //执行下一个节点
            dialogueComponent.RemoveTag(DialogueTag.CanEnterSetting);

            return Status.Success;
        }
    }
}