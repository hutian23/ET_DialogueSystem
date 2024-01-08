using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ET.Client
{
    [NodeEditorOf(typeof (GotoNode))]
    public sealed class GotoNodeView: DialogueNodeView
    {
        private readonly IntegerField intField;

        public GotoNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);

            intField = new IntegerField("选择跳转的节点: ");
            GotoNode gotoNode = dialogueNode as GotoNode;
            intField.SetValueWithoutNotify(gotoNode.Goto_targetID);
            intField.RegisterCallback<BlurEvent>(_ => treeView.SetDirty());

            contentContainer.Add(intField);
            GenerateDescription();

            SaveCallback += Save;
        }
        
        private void Save()
        {
            GotoNode gotoNode = node as GotoNode;
            gotoNode.Goto_targetID = intField.value;
        }
    }
}