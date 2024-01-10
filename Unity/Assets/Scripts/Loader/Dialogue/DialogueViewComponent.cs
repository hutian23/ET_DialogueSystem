using ET.Client;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class DialogueViewComponent: MonoBehaviour
    {
        [LabelText("引用: ")]
        [ReadOnly]
        public DialogueTree tree;

        [ReadOnly]
        [LabelText("克隆树: ")]
        public DialogueTree cloneTree;
    }
}