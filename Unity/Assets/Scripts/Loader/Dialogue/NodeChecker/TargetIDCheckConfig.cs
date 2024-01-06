using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [HideReferenceObjectPicker]
    public class TargetCheck
    {
        //是否是外部的对话树
        public bool IsGlobal;
        [ShowIf("IsGlobal")]
        public int treeId;
        public int targetID;
    }
    
    public class TargetIDCheckConfig : NodeCheckConfig
    {
        [InfoBox("检查前置节点是否已经执行")]
        public List<TargetCheck> checkList = new();
    }
}