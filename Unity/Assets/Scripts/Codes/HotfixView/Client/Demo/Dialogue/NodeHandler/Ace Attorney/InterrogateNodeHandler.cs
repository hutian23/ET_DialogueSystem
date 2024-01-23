using UnityEngine.InputSystem;

namespace ET.Client
{
    public class InterrogateNodeHandler: NodeHandler<InterrogateNode>
    {
        protected override async ETTask<Status> Run(Unit unit, InterrogateNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            ETCancellationToken curNodeToken = new();
            token.Add(curNodeToken.Cancel);
            
            SkipCor(dialogueComponent, node, curNodeToken).Coroutine();
            await TimerComponent.Instance.WaitAsync(5000, curNodeToken);

            if (token.IsCancel()) return Status.Failed; //注意区别 1.热重载
            if (curNodeToken.IsCancel()) return Status.Success; //2. 回退上一个节点

            node.nexts.ForEach(next => dialogueComponent.PushNextNode(next));
            token.Remove(curNodeToken.Cancel);
            return Status.Success;
        }

        //回退上一个节点
        private async ETTask SkipCor(DialogueComponent dialogueComponent, InterrogateNode node, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(300, token);
            if (token.IsCancel()) return;
            while (true)
            {
                if (token.IsCancel()) break;
                if (Keyboard.current.qKey.wasPressedThisFrame)
                {
                    dialogueComponent.PushNextNode(node.preNode);
                    token.Cancel();
                    break;
                }

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    foreach (var next in node.nexts)
                    {
                        dialogueComponent.PushNextNode(next);
                        token.Cancel();
                        break;
                    }
                    break;
                }

                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}