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
            BehaviorBufferComponent bufferComponent = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            bufferComponent.EnableBufferCheck(token);

            //3. 执行行为
            long currentOrder = FTGHelper.GetOrder(BehaviorOrder.Normal, 0);
            while (true)
            {
                uint targetID = bufferComponent.GetTargetID(currentOrder);
                if (dialogueComponent.GetNode(targetID) is not BBNode bbNode)
                {
                    Log.Error($"cannot node TargetID: {targetID} is not a BBNode");
                    return Status.Failed;
                }
                
                dialogueComponent.SetNodeStatus(bbNode, Status.Pending);
                await DialogueDispatcherComponent.Instance.Handle(unit, bbNode, token);
                if (token.IsCancel()) return Status.Failed;
                dialogueComponent.SetNodeStatus(bbNode, Status.None);
                
                //等待执行下一个行为
                ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();
                WaitNextBehavior wait = await objectWait.Wait<WaitNextBehavior>(token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
                currentOrder = wait.order;
                
                //这里是我怕死循环了，过一帧再执行
                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return Status.Failed;
            }
        }
    }
}