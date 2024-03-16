namespace ET.Client
{
    [FriendOf(typeof (BBInputComponent))]
    public class BBRootHandler: NodeHandler<BBRoot>
    {
        protected override async ETTask<Status> Run(Unit unit, BBRoot node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            BBParser Parser = dialogueComponent.GetComponent<BBParser>();

            //1. 初始化 生成特效对象池 注册必杀按键检测 注册变量等
            for (uint i = 0; i < dialogueComponent.GetLength(); i++)
            {
                DialogueNode childNode = dialogueComponent.GetNode(i);
                if (childNode is not BBNode bbNode) continue;
                Parser.InitScript(bbNode);
                await Parser.Init();
            }

            //2. 启动行为缓冲组件
            BehaviorBufferComponent behaviorBuffer = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            behaviorBuffer.EnableBufferCheck(token);

            //3. 执行行为
            IdleCor(unit, token).Coroutine();
            while (true)
            {
                ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();
                WaitNextBehavior wait = await objectWait.Wait<WaitNextBehavior>(token);
                if (token.IsCancel()) return Status.Failed;

                uint targetID = behaviorBuffer.GetTargetID(wait.order);
                if (dialogueComponent.GetNode(targetID) is not BBNode bbNode)
                {
                    Log.Error($"cannot convert {targetID} to bbNode");
                    return Status.Failed;
                }

                dialogueComponent.SetNodeStatus(bbNode, Status.Pending);
                await DialogueDispatcherComponent.Instance.Handle(unit, bbNode, token);
                if (token.IsCancel()) return Status.Failed;
                dialogueComponent.SetNodeStatus(bbNode, Status.None);

                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return Status.Failed;
            }
        }

        private async ETTask IdleCor(Unit unit, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitFrameAsync(token);
            if (token.IsCancel()) return;

            long order = FTGHelper.GetOrder(BehaviorOrder.Normal, 0); //Idle
            unit.GetComponent<DialogueComponent>().GetComponent<ObjectWait>().Notify(new WaitNextBehavior() { order = order });
        }
    }
}