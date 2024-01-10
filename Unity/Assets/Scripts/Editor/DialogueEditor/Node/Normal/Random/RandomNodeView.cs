using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof(RandomNode))]
    public class RandomNodeView : DialogueNodeView
    {
        private readonly Port randomPort;
        public RandomNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("", true);
            randomPort = this.GenerateOutputPort("随机", true);
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if(node is not RandomNode randomNode) return;
            randomNode.random = this.GetLinkNodes(randomPort);
        }
    }
}