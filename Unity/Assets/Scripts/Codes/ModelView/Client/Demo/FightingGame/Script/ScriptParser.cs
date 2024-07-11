using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    // [ChildOf(typ)]
    public class ScriptParser: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, int> funcMap = new();
        public Dictionary<string, int> markerMap = new();

        public string opLines;
        public Dictionary<int, string> opDict = new();

        public ETCancellationToken Token;
        public Dictionary<string, SubCoroutineData> subCoroutineDatas = new();
    }

    [Serializable]
    public class SubCoroutineData
    {
        public string coroutineName; //协程名
        public int pointer; //当前函数指针
        public ETCancellationToken token; //取消当前协程

        public static SubCoroutineData Create(string name, int startPointer, ETCancellationToken token)
        {
            SubCoroutineData subCoroutineData = ObjectPool.Instance.Fetch<SubCoroutineData>();
            subCoroutineData.coroutineName = name;
            subCoroutineData.pointer = startPointer;
            subCoroutineData.token = token;
            return subCoroutineData;
        }

        public void Recycle()
        {
            coroutineName = string.Empty;
            pointer = 0;
            token.Cancel();
            ObjectPool.Instance.Recycle(this);
        }
    }

    [Serializable]
    public class ScriptData
    {
        public string opLine; // 指令码
        public string coroutineID; // 协程ID  --- > main协程

        public static ScriptData Create(string opLine, string coroutineID)
        {
            ScriptData scriptData = ObjectPool.Instance.Fetch<ScriptData>();
            scriptData.opLine = opLine;
            scriptData.coroutineID = coroutineID;
            return scriptData;
        }

        public void Recycle()
        {
            opLine = string.Empty;
            coroutineID = string.Empty;
            ObjectPool.Instance.Recycle(this);
        }
    }
}