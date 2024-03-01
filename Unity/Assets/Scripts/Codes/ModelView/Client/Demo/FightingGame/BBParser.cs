using System.Collections.Generic;

namespace ET.Client
{
    //运行时解析BBScript然后执行
    [ComponentOf(typeof (DialogueComponent))]
    public class BBParser: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string,int> funcMap = new();// 记录状态块的索引
        public string opLines; // 脚本
        public ETCancellationToken cancellationToken; //取消当前执行的所有子协程
    }
}