namespace ET.Client
{
    public class BBRootHandler: NodeHandler<BBRoot>
    {
        protected override async ETTask<Status> Run(Unit unit, BBRoot node, ETCancellationToken token)
        {
            foreach (uint targetID in node.behaviors)
            {
                DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
                BBParser parser = dialogueComponent.GetComponent<BBParser>();
                //初始化
                // 生成特效对象池 注册必杀按键检测 注册变量等
                BBNode child = dialogueComponent.GetNode(targetID) as BBNode;
                parser.InitScript(child.BBScript);
                await parser.Init(token);
            }

            while (true)
            {
                if (token.IsCancel()) return Status.Failed;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}