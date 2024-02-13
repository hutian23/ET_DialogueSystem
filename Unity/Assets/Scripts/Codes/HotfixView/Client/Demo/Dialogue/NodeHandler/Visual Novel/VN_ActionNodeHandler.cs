using UnityEngine.InputSystem;

namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            //1. 打印携程
            DialogueHelper.ReplaceModel(unit, ref node.text);
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            await dialogueComponent.TypeCor(dlgDialogue.View.E_TextText, node.text, token);
            if (token.IsCancel()) return Status.Failed;

            //2. 等待下一个节点入队
            WaitNextCor(dialogueComponent, token).Coroutine();
            await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitNextNode>(token);
            if (token.IsCancel()) return Status.Failed;
            dlgDialogue.RefreshArrow(); // 隐藏箭头

            //3. 执行下一个节点
            dialogueComponent.PushNextNode(dialogueComponent.GetFirstNode(node.children));

            return Status.Success;
        }

        private static async ETTask WaitNextCor(DialogueComponent self, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(200, token);
            if (token.IsCancel()) return;

            //取消等待按键触发的携程
            ETCancellationToken WaitKeyPressedToken = new();
            token.Add(WaitKeyPressedToken.Cancel);

            //刷新UI,显示右箭头
            DlgDialogue dlgDialogue = self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.RefreshArrow();
            dlgDialogue.ShowRightArrow(() =>
            {
                self.GetComponent<ObjectWait>().Notify(new WaitNextNode());
                WaitKeyPressedToken.Cancel();
            });
            
            while (true)
            {
                if (WaitKeyPressedToken.IsCancel()) return;
                if (Keyboard.current.spaceKey.isPressed)
                {
                    self.GetComponent<ObjectWait>().Notify(new WaitNextNode());
                    token.Remove(WaitKeyPressedToken.Cancel);
                    return;
                }

                await TimerComponent.Instance.WaitFrameAsync(WaitKeyPressedToken);
            }
        }
    }
}