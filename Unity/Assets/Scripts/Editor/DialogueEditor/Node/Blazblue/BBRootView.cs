using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public class BBRootView: DialogueNodeView<BBRoot>
    {
        public BBRootView(BBRoot dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("");
            Port port = GenerateOutputPort("", true);
            this.SaveCallback += () => { dialogueNode.behaviors = GetLinkNodes(port); };
        }
    }
}