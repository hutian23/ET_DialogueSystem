using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class Angry_ChoiceNodeView: DialogueNodeView<Angry_ChoiceNode>
    {
        public Angry_ChoiceNodeView(Angry_ChoiceNode node, DialogueTreeView treeView): base(node, treeView)
        {
            GenerateInputPort("", true);
            Port Angry = GenerateOutputPort("in anger", true);
            Port Normal = GenerateOutputPort("normal", true);

            SaveCallback += () =>
            {
                node.Angrys = GetLinkNodes(Angry);
                node.Normal = GetLinkNodes(Normal);
            };
        }
    }
}