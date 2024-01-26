using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class GotoNodeView: DialogueNodeView<GotoNode>
    {
        public GotoNodeView(GotoNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);

            IntegerField intField = new("选择跳转的节点: ");
            intField.SetValueWithoutNotify(dialogueNode.Goto_targetID);
            intField.RegisterCallback<BlurEvent>(_ => treeView.SetDirty());

            contentContainer.Add(intField);
            SaveCallback += () => { dialogueNode.Goto_targetID = intField.value; };
        }
    }
}