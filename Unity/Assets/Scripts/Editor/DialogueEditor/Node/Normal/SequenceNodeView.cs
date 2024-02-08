using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class SequenceNodeView: DialogueNodeView<SequenceNode>
    {
        public SequenceNodeView(SequenceNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("");
            Port port = GenerateOutputPort("", true);
            this.SaveCallback += () => { dialogueNode.children = GetLinkNodes(port); };
        }
    }
}