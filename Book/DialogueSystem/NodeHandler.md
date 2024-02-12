两方面的参考: 

[Arc引擎脚本的草记（中文翻译）来自pangaea的博客分享](https://www.bilibili.com/read/cv16416914/?from=search&spm_id_from=333.337.0.0)

[ObjectionMaker](https://objection.lol/maker)

我们先简单介绍一下行为树的一些概念:
运行流程: 从根节点自顶往下遍历，每经过一个节点就执行节点对应的功能。
根据节点职责可以简单划分成条件节点、动作节点以及组合节点。包含一些限制条件，例如叶子节点只能是条件节点或动作节点，分支节点必须是组合节点。


我们以ECS的方式重构对话树。

我很喜欢ObjectionMaker中的一个设计，每个节点就是对话中的一帧，同时只有一个节点，也就是只有一帧正在执行，
你可以控制从哪一帧进入对话。

一个节点等于一个Entity,

BBScript优点: 
1. 方便热重载.
2. 主流行为树多将一些行为单独分装成一个节点，比如，Debug.Log会分装成一个Log节点，播放动画一个节点。
观感上其实就很不舒服，一个父节点下有10几个叶子节点，这谁受得了。

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