using Sirenix.OdinInspector;

namespace ET.Client
{
    public class RootNode : DialogueNode
    {
        [FoldoutGroup("$nodeName")]
        public int nextNode;
    }
}