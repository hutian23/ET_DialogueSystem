using ET.Client;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public struct ViewComponentReloadCallback
    {
        public long instanceId;
    }
    
    public class DialogueViewComponent: MonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        [LabelText("语言: ")]
        public Language Language;

        [LabelText("引用: ")]
        public DialogueTree tree;

        [LabelText("克隆树: ")]
        public DialogueTree cloneTree;
        
        public DialogueNode GetNode(uint targetID)
        {
            if (cloneTree == null)
            {
                Debug.LogError("DialogueViewComponent cloneTree is null");
                return null;
            }

            cloneTree.targets.TryGetValue(targetID, out DialogueNode node);
            node.text = node.GetContent(Language);
            return node;
        }
    }
}