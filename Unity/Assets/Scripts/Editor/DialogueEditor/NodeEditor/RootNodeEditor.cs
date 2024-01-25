using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class RootNodeEditor : NodeEditorBase<RootNode>
    {
        public Port test;

        public RootNodeEditor(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            capabilities &= ~ Capabilities.Movable;
            capabilities &= ~ Capabilities.Deletable;

            this.title = "根节点";
            GenerateOutputPort("start");
        }
    }
}