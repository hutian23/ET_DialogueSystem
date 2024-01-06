using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ET.Client
{
    [NodeType("Disco/分支/愤怒节点（延时显示其他选项）")]
    public class Angry_ChoiceNode: DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        [OdinSerialize, LabelText("愤怒"), HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<int> Angrys = new();
        [FoldoutGroup("$nodeName")]
        [OdinSerialize, LabelText("情绪正常"), HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<int> Normal = new();
    }
}