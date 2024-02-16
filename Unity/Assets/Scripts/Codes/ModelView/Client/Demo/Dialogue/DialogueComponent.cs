using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class DialogueComponent: Entity, IAwake, IDestroy, ILoad
    {
        public ETCancellationToken token;
        public Queue<DialogueNode> workQueue = new();

        public DialogueTreeData treeData;
        public int ReloadType;

        public HashSet<int> tags = new();
        public List<SharedVariable> Variables = new();
    }

    public struct LoadTreeCallback
    {
        public long instanceId;
        public int ReloadType;
        public uint treeID;
        public uint targetID;
        public Language language;
    }

    [UniqueId(0, 10000)]
    public static class DialogueTag
    {
        public const int None = 0;
        public const int TypeCor = 1; //当前在打字携程中
        public const int Typing = 2; //正在打字
        public const int InDialogueCor = 3;
        public const int CanEnterSetting = 4; // 可以切换到设定界面，打字协程中不能切换
    }
}