using System.Collections.Generic;

namespace ET.Client
{
    //运行时解析BBScript然后执行
    [ComponentOf(typeof (DialogueComponent))]
    public class BBParser: Entity, IAwake, IDestroy, ILoad
    {
        //记录状态块的索引
        public Dictionary<string,int> funcMap = new();
    }
}