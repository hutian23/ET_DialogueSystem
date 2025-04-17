using System;
using System.Collections.Generic;

namespace ET.Client
{
    //运行时解析BBScript然后执行
    [ComponentOf]
    public class BBParser: Entity, IAwake, IDestroy, ILoad, IController
    {
        public Dictionary<int, string> OpDict = new();
        public Dictionary<string, DataGroup> GroupDict = new();
        public HashSet<int> GroupPointerSet = new();
        
        public ETCancellationToken CancellationToken; // 热重载时取消所有BBParser子协程
        public Dictionary<long, int> Coroutine_Pointers = new(); // 协程ID --> 协程指针
        public Dictionary<string, SharedVariable> ParamDict = new(); // 在携程内注册变量，携程执行完毕dispose
        public Dictionary<string, BBValue> ValueDict = new();
    }
    
    public struct ProcessBBScriptCallback
    {
        public long instanceId;
    }

    public struct ReplaceParamCallback
    {
        public long instanceId;
        public string content;
    }
    
    public class BBScriptData
    {
        public string opLine; //指令码
        public long CoroutineID; //协程ID

        public static BBScriptData Create(string opLine, long functionID)
        {
            BBScriptData scriptData = ObjectPool.Instance.Fetch<BBScriptData>();
            scriptData.opLine = opLine;
            scriptData.CoroutineID = functionID;
            return scriptData;
        }

        public void Recycle()
        {
            opLine = string.Empty;
            CoroutineID = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }
    

    #region DataGroup
    public class DataGroup
    {
        public string groupName;
        // 维护代码块起始、结束索引
        public int startIndex;
        public int endIndex;
        // 代码块内函数头指针
        public Dictionary<string, int> funcPointers = new();
        public Dictionary<string, int> markerPointers = new();
        
        public static DataGroup Create()
        {
            DataGroup dataGroup = ObjectPool.Instance.Fetch<DataGroup>();
            dataGroup.Recycle();
            return ObjectPool.Instance.Fetch<DataGroup>();
        }

        public void Recycle()
        {
            groupName = string.Empty;
            startIndex = 0;
            endIndex = 0; 
            funcPointers.Clear();
            markerPointers.Clear();
        }
    }
    
    #endregion
    
    #region If
    public enum SyntaxType
    {
        None,
        Condition,
        Normal
    }

    [Serializable]
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
    #endregion
}