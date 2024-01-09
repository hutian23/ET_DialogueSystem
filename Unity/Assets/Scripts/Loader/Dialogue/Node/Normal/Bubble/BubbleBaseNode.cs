using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ET.Client
{
    [NodeType("常用节点/气泡/气泡基类节点")]
    public class BubbleBaseNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [OdinSerialize, LabelText("气泡列表"), HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        [ReadOnly]
        public List<uint> bubbles = new();

        public override DialogueNode Clone()
        {
            bubbles.Clear();
            return base.Clone();
        }
    }
}