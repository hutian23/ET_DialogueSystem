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
        [LabelText("要求目标节点: ")]
        public TargetCheckType CheckType = TargetCheckType.已执行;
        
        [LabelText("对话树ID: ")]
        public uint treeID = 0;

        [LabelText("目标节点ID: ")]
        public uint targetID = 0;
    }

    public class TargetIDCheckConfig: NodeCheckConfig
    {
        [InfoBox("检查前置节点是否已经执行")]
        public List<TargetCheck> checkList = new();
    }
}