using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class RandomNodeView : DialogueNodeView<RandomNode>
    {
        public RandomNodeView(RandomNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            Port randomPort = GenerateOutputPort("随机", true);
            SaveCallback += () => { dialogueNode.random = GetLinkNodes(randomPort);};
        }
    }
}