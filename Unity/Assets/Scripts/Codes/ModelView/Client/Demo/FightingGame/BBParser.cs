using System.Collections.Generic;

namespace ET.Client
{
    //运行时解析BBScript然后执行
    [ComponentOf(typeof (DialogueComponent))]
    public class BBParser: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, int> funcMap = new(); // 记录状态块的索引
        public Dictionary<string, int> markers = new(); //标记位置
        public string opLines; // 脚本
        public Dictionary<int, string> opDict = new();
        public ETCancellationToken cancellationToken; //取消当前执行的所有子协程

        public uint currentID; // 当前执行的节点ID

        //协程ID --> 协程指针
        public Dictionary<long, int> function_Pointers = new();
    }

    public class BBScriptData
    {
        public string opLine; //指令码
        public long functionID; //协程ID
        public uint targetID; // 节点ID
        
        public static BBScriptData Create(string opLine, long functionID,uint targetID)
        {
            BBScriptData scriptData = ObjectPool.Instance.Fetch<BBScriptData>();
            scriptData.opLine = opLine;
            scriptData.functionID = functionID;
            scriptData.targetID = targetID;
            return scriptData;
        }

        public void Recycle()
        {
            this.opLine = string.Empty;
            this.functionID = 0;
            this.targetID = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public enum SyntaxType
    {
        None,
        Condition,
        Normal
    }

    public class SyntaxNode
    {
        public List<SyntaxNode> children = new();
        public SyntaxType nodeType;
        public int index;
        public int endIndex;

        public static SyntaxNode Create(SyntaxType nodeType, int index)
        {
            SyntaxNode node = ObjectPool.Instance.Fetch<SyntaxNode>();
            node.nodeType = nodeType;
            node.index = index;
            return node;
        }

        public void Recycle()
        {
            this.nodeType = SyntaxType.None;
            this.index = 0;
            this.endIndex = 0;
            this.children.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
}