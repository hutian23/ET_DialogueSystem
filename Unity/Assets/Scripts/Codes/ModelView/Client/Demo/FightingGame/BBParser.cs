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
        public ETCancellationToken cancellationToken; //取消当前执行的所有子协程
        public uint currentID; // 当前执行的节点ID

        //协程ID --> 协程指针
        public Dictionary<long, int> function_Pointers = new();
    }

    public class BBScriptData
    {
        public string opLine; //指令码
        public long functionID; //协程ID

        public static BBScriptData Create(string opLine, long functionID)
        {
            BBScriptData scriptData = ObjectPool.Instance.Fetch<BBScriptData>();
            scriptData.opLine = opLine;
            scriptData.functionID = functionID;
            return scriptData;
        }

        public void Recycle()
        {
            this.opLine = string.Empty;
            this.functionID = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public enum SyntaxType
    {
        Condition,
        Normal
    }

    public class SyntaxNode
    {
        public string opLine;
        public List<SyntaxNode> children = new();
        public SyntaxType nodeType;
        public int index;
    }
}