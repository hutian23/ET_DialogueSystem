using UnityEngine.InputSystem;

namespace ET.Client
{
    public class InterrogateNodeHandler: NodeHandler<InterrogateNode>
    {
        protected override async ETTask<Status> Run(Unit unit, InterrogateNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            //1. 打印携程
            DialogueHelper.ReplaceModel(unit, ref node.text);
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            await dialogueComponent.TypeCor(dlgDialogue.View.E_TextText, node.text, token);
            if (token.IsCancel()) return Status.Failed;

            //3. 等待下一个节点入队
            WaitNextCor(unit, node, token).Coroutine();
            WaitChoiceNode waitChoice = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitChoiceNode>(token);
            if (token.IsCancel()) return Status.Failed;
            unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>().RefreshArrow();
            dialogueComponent.PushNextNode(waitChoice.next);
            
            return Status.Success;
        }

        private static async ETTask WaitNextCor(Unit unit, InterrogateNode node, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(200, token);
            if (token.IsCancel()) return;
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            uint next = dialogueComponent.GetFirstNode(node.nexts);
            
            //刷新UI
            DlgDialogue dlgDialogue = unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.RefreshArrow();
            dlgDialogue.ShowRightArrow(()=>{ dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode(){next = next});});
            if (node.CanGoBack) dlgDialogue.ShowLeftArrow(() => { dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode() { next = node.Goto }); });

            ETCancellationToken WaitNextToken = new();
            token.Add(WaitNextToken.Cancel);
            
            while (true)
            {
                if (WaitNextToken.IsCancel()) return;
                if (Keyboard.current.spaceKey.isPressed)
                {
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode(){next = next});
                    token.Remove(WaitNextToken.Cancel);
                    return;
                }
                if (Keyboard.current.qKey.isPressed) //追问
                {
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode(){next = node.hold_it});
                    token.Remove(WaitNextToken.Cancel);
                    return;
                }
                await TimerComponent.Instance.WaitFrameAsync(WaitNextToken);
            }
        }
    }
}