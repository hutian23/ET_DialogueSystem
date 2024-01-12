using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET.Client
{
    public enum TargetCheckType
    {
        已执行,
        未执行
    }

    [HideReferenceObjectPicker]
    public class TargetCheck
    {
        public TargetCheckType CheckType = TargetCheckType.已执行;

        //是否是外部的对话树
        public bool IsGlobal;

        [ShowIf("IsGlobal")]
        public uint treeID = 0;

        public uint targetID = 0;
    }

    public class TargetIDCheckConfig: NodeCheckConfig
    {
        [InfoBox("检查前置节点是否已经执行")]
        public List<TargetCheck> checkList = new();
    }
}