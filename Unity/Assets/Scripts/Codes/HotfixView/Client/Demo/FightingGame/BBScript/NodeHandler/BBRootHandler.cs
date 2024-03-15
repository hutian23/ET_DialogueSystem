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
                await Parser.Init(token);
            }
            
            //2. 启动行为缓冲组件
            BehaviorBufferComponent behaviorBuffer = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            behaviorBuffer.EnableBehaviorBufferCheck();
            return Status.Success;
        }
    }
}