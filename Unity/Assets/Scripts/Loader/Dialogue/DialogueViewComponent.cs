using ET.Client;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public struct ViewComponentReloadCallback
    {
        public long instanceId;
        public int ReloadType;
        public uint preView_TargetID;
    }

    public static class ViewReloadType
    {
        public const int None = 0;
        public const int Reload = 1; //重新加载对话树，从头开始
        public const int Preview = 2; // 预览单个节点，需要执行rootNode的初始化
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