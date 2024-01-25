using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class VN_ActionNodeEditor: NodeEditorBase<VN_ActionNode>
    {
        public VN_ActionNodeEditor(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port port = this.GenerateOutputPort("", true);
            this.SaveCallback += () => { node.children = GetLinkNodes(port); };
        }
    }
}