using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class TimelineReloadCallback: AInvokeHandler<BBTestManagerCallback>
    {
        public override void Handle(BBTestManagerCallback args)
        {
            DialogueComponent dialogueComponent = Root.Instance.Get(args.instanceId) as DialogueComponent;
            if (dialogueComponent == null)
            {
                return;
            }

            ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();
            objectWait.Notify(new WaitStopCurrentBehavior() { order = args.order });
            if (args.stop == 0)
            {
                objectWait.Notify(new WaitNextBehavior() { order = args.order });   
            }
        }
    }

    [FriendOf(typeof (BBInputComponent))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (BehaviorInfo))]
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

            //3. Loader层调试timeline
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            token.Add(() => { UnityEngine.Object.DestroyImmediate(go.GetComponent<BBTestManager>()); });
            UnityEngine.Object.DestroyImmediate(go.GetComponent<BBTestManager>());

            BBTestManager testManager = go.AddComponent<BBTestManager>();
            testManager.instanceId = dialogueComponent.InstanceId;
            testManager.dropdownDict.Clear();
            foreach (var behaviorInfo in bufferComponent.behaviorDict.Values)
            {
                testManager.dropdownDict.Add($"{behaviorInfo.order} --- {behaviorInfo.behaviorName}", (int)behaviorInfo.order);
            }

            //4. 执行行为
            long currentOrder = 0;
            while (true)
            {
                //根据Order找到对应节点
                uint targetID = bufferComponent.GetTargetID(currentOrder);
                if (dialogueComponent.GetNode(targetID) is not BBNode bbNode)
                {
                    Log.Error($"cannot node TargetID: {targetID} is not a BBNode");
                    return Status.Failed;
                }

                ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();

                //支持 TestManager 取消当前行为(比如一个loop的行为，这里就一直不会往下执行)，预览下一个行为
                async ETTask WaitStopCurrentBehaviorCor()
                {
                    await objectWait.Wait<WaitStopCurrentBehavior>(token);
                    if (token.IsCancel())
                    {
                        return;
                    }
                    dialogueComponent.GetComponent<BBParser>().Cancel();
                }
                WaitStopCurrentBehaviorCor().Coroutine();

                dialogueComponent.SetNodeStatus(bbNode, Status.Pending);
                await DialogueDispatcherComponent.Instance.Handle(unit, bbNode, token);

                if (token.IsCancel())
                {
                    return Status.Failed;
                }

                dialogueComponent.SetNodeStatus(bbNode, Status.None);

                //等待执行下一个行为
                WaitNextBehavior wait = await objectWait.Wait<WaitNextBehavior>(token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }

                currentOrder = wait.order;
                //这里是我怕死循环了，过一帧再执行
                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
            }
        }
    }
}