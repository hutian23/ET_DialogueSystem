# 对话系统开发笔记
本篇主要记录以下我开发对话系统中的心得和思考，一边写一边整理一下思路(毕竟磨洋工做了两个月，不总结一下都不知道自己做了啥了...)

两方面的参考: 

[Arc引擎脚本的草记（中文翻译）来自pangaea的博客分享](https://www.bilibili.com/read/cv16416914/?from=search&spm_id_from=333.337.0.0)

[ObjectionMaker](https://objection.lol/maker)

## 简单介绍行为树

我们先简单介绍一下行为树的一些概念。
运行流程: 从根节点自顶往下遍历，每经过一个节点就执行节点对应的功能。
根据节点职责可以简单划分成条件节点、动作节点以及组合节点。包含一些限制条件，例如叶子节点只能是条件节点或动作节点，分支节点必须是组合节点。


## 常见行为树有什么缺点

每个方法都要封装一个节点，节点太多。比如，Debug.Log会分装成一个Log节点，播放动画一个节点。
观感上其实就很不舒服，一个父节点下有10几个叶子节点，这谁受得了。

树太大，不好编辑。
表达性弱，难以阅读、重构。
协程支持比较原始。一般行为树会把一个节点的执行状态分成Failed,Success,Running三种状态。进入节点执行Start(),节点运行时执行Running(),退出节点执行Exit()...对异步的支持比较弱，可能需要额外封装一个WaitTime节点。

## 改进
1. 以帧为单位的行为树

我很喜欢ObjectionMaker中的一个设计，每个节点就是对话中的一帧，同时只有一个节点，也就是只有一帧正在执行，
你可以控制从哪一帧进入对话。

以ECS的思想来设计行为树，每个节点都是一个Entity，节点中包含条件(CheckerConfig)

运行时，每个节点相当于一个协程。

- NodeView(编辑器)
- NodeHandler(逻辑层)
- Node(数据层)

以下是对话携程的主函数:
```csharp
private static async ETTask DialogueCor(this DialogueComponent self)
{
    await TimerComponent.Instance.WaitFrameAsync(); // 意义?: 等待所有reload生命周期事件执行完毕
    if (Application.isEditor) self.ViewStatusReset();

    DialogueNode node = self.GetNode(0); //压入根节点
    self.workQueue.Enqueue(node);
    Unit unit = self.GetParent<Unit>();

    try
    {
        while (self.workQueue.Count != 0)
        {
            if (self.token.IsCancel()) break;
            node = self.workQueue.Dequeue(); //将下一个节点压入queue执行

            self.SetNodeStatus(node, Status.Pending);
            Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);//执行节点
            self.SetNodeStatus(node, ret);

            if (self.token.IsCancel() || ret == Status.Failed) break; //携程取消 or 执行失败
            await TimerComponent.Instance.WaitFrameAsync(self.token);
        }
    }
    catch (Exception e)
    {
        Log.Error(e);
    }
}
```
只关心节点是否执行失败(其他的状态都是编辑器中可视化节点执行结果的，运行时不关心)和对话携程是否被取消。

将Start(),Update(),Exit()合成一个函数。
```csharp
namespace ET.Client
{
    public class RootNodeHandler: NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            token.Add(() => { Log.Warning("携程被取消"); }); //携程被取消的回调
            await TimerComponent.Instance.WaitAsync(3000, token);
            if (token.IsCancel()) return Status.Failed; // 携程被取消，就不往后面执行了
            Log.Warning("Hello world");

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.nextNode);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}
```

加强了对异步的支持，也支持协程的取消。热重载对话树的时候，我们可以很容易的取消对话树协程和当前运行的节点子协程，并从根节点进入重新执行对话协程。


异步的更多花样

```csharp
//对ObjectWait不了解的可以看这篇: https://et-framework.cn/d/351-objectwaitentity
namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            WaitNextCor(dialogueComponent, token).Coroutine();
            
            //1. 等待UI点击事件，确认后执行之后的逻辑
            await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitNextNode>(token);
            if (token.IsCancel()) return Status.Failed;
            dlgDialogue.RefreshArrow(); // 隐藏箭头

            //2. 执行下一个节点
            dialogueComponent.PushNextNode(dialogueComponent.GetFirstNode(node.children));
            return Status.Success;
        }

        private static async ETTask WaitNextCor(DialogueComponent self, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(200, token);
            if (token.IsCancel()) return;

            //取消等待按键触发的协程
            ETCancellationToken WaitKeyPressedToken = new();
            token.Add(WaitKeyPressedToken.Cancel);

            //刷新UI,显示右箭头
            DlgDialogue dlgDialogue = self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgDialogue>();
            dlgDialogue.RefreshArrow();
            dlgDialogue.ShowRightArrow(() =>
            {
                self.GetComponent<ObjectWait>().Notify(new WaitNextNode());
                WaitKeyPressedToken.Cancel(); // 触发点击事件后，取消检测按键协程
            });

            //检测按键
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
```

BBScript优点: 

1. 方便热重载.

行为树的节点，本质上是将代码封装成一个个节点，这样做，使得节点太多，树太大，不好编辑。

既然ET支持热重载，我的想法是不如把方法封装成指令，运行时可以对指令进行热重载。

项目中的脚本，其实有点像Lua(虽然我没学过，哈哈)。通过指令访问到Entity树上的数据，或者对数据进行操作。
其中ScriptHandler参考的是ET的BehaviorHandler,支持运行时热重载。

```csharp
namespace ET.Client
{
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class DialogueDispatcherComponentSystem
    {
        public class DialogueDispatcherComponentLoadSystem: LoadSystem<DialogueDispatcherComponent>
        {
            protected override void Load(DialogueDispatcherComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this DialogueDispatcherComponent self)
        {
            self.checker_dispatchHandlers.Clear();
            var nodeCheckerHandlers = EventSystem.Instance.GetTypes(typeof (NodeCheckerAttribute));
            foreach (Type type in nodeCheckerHandlers)
            {
                NodeCheckHandler nodeCheckHandler = Activator.CreateInstance(type) as NodeCheckHandler;
                if (nodeCheckHandler == null)
                {
                    Log.Error($"this obj is not a nodeCheckerHandler:{type.Name}");
                    continue;
                }

                self.checker_dispatchHandlers.Add(nodeCheckHandler.GetNodeCheckType(), nodeCheckHandler);
            }
        }
    }
}
```

本项目中把一些事件都封装成指令。

```csharp
ShowWindow type = Dialogue;

# 注册演员
VN_RegistCharacter ch = Skye unitId = 1003;
VN_HideCharacter ch = Skye;
VN_RegistCharacter ch = Phoniex unitId = 1003;
VN_HideCharacter ch = Phoniex;

# 注册背景
VN_RegistBackground name = Witness;

# 注册共享变量
RegistRandomVariable min = <Constant name=Min/> max = <Constant name=Max/>;
# Numeric Hp + <Variable name=Random/>;

```

如果你想，把一些指令符合变成一个函数也是完全可以的。

```csharp
namespace ET.Client
{
    public class HoldIt_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "HoldIt()";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            var opLines = "HideWindow type = Dialogue;\nVN_RegistEffect name = hold_it prefabName = HoldIt;\nVN_Shake effect = hold_it curve = ShakeCurve duration = ShakeDuration intensity = ShakeIntensity;\nVN_RemoveEffect name = hold_it;\nWaitTime 500;\nShowWindow type = Dialogue;";
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, opLines, token);
        }
    }
}

HoldIt();
```

## 数据
运行时支持访问3种类型的数据:
1. 常量,对话树中。
2. 运行时注册的变量。
3. 其他组件上的数据，例如数值组件。

```csharp
RegistRandomVariable min = <Constant name=Min/> max = <Constant name=Max/>;

Numeric Hp + <Variable name=Random/>;
```

关于变量这个，算是构思的比较久的。


```csharp
 [HideReferenceObjectPicker]
    public abstract class DialogueNode: Object
    {
        [HideInInspector, ReadOnly]
        public uint TreeID;

        [HideInInspector, ReadOnly]
        public uint TargetID;

        [FoldoutGroup("$nodeName"), LabelText("检查前置条件: ")]
        public bool NeedCheck;
        
        [FoldoutGroup("$nodeName"),Space(5),ShowIf("$NeedCheck")]
        public List<NodeCheckConfig> checkList = new();

        [FoldoutGroup("$nodeName"), LabelText("显示脚本: "),Space(5)]
        [BsonIgnore]
        public bool ShowScript;

        [FoldoutGroup("$nodeName"), HideLabel, TextArea(10, 35), ShowIf("ShowScript")]
        public string Script = "";

        [HideInInspector, BsonIgnore]
        public string text;

#if UNITY_EDITOR
        [HideInInspector]
        public string Guid;

        [HideInInspector]
        public Vector2 position;

        [BsonIgnore]
        [HideInInspector, ReadOnly, FoldoutGroup("$nodeName")]
        public Status Status;
        
        [Searchable]
        [FoldoutGroup("$nodeName"), HideReferenceObjectPicker, LabelText("本地化组"), Space(10),
         ListDrawerSettings(ShowFoldout = true, ShowIndexLabels = true, ListElementLabelName = "eleName")]
        public List<LocalizationGroup> LocalizationGroups = new();

        public string nodeName => $"[{TargetID}]{GetType().Name}";

        public string GetContent(Language language)
        {
            //所以我明明new了，为什么odin还会显示null呢?
            if (LocalizationGroups == null) return String.Empty;
            var targetGroup = LocalizationGroups.FirstOrDefault(group => group.Language == language);
            return targetGroup == null? String.Empty : targetGroup.content;
        }

        public virtual DialogueNode Clone()
        {
            DialogueNode cloneNode = MongoHelper.Clone(this);
            cloneNode.TargetID = 0;
            cloneNode.TreeID = 0;
            cloneNode.Guid = GUID.Generate().ToString();
            return cloneNode;
        }
#endif
        //注意MongoBson只支持signed int64
        public long GetID()
        {
            ulong result = 0;
            result |= TargetID;
            result |= (ulong)TreeID << 32;
            return (long)result;
        }

        //ID转成treeID和TargetID
        public void FromID(long ID)
        {
            ulong result = (ulong)ID;
            TargetID = (uint)(result & uint.MaxValue);
            result >>= 32;
            TreeID = (uint)(result & uint.MaxValue);
        }
    }

#if UNITY_EDITOR
    [Serializable]
    public class LocalizationGroup
    {
        [LabelText("语言: "), Space(10)]
        public Language Language = Language.Chinese;

        public string eleName => Language.ToString();

        [TextArea(3, 4), Space(10)]
        [HideLabel]
        public string content = "";
    }
#endif

    public class NodeTypeAttribute: BaseAttribute
    {
        public string Level;

        public NodeTypeAttribute(string level)
        {
            this.Level = level;
        }
    }
}
```