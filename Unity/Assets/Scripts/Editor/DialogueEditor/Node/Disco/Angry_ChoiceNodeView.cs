using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Angry_ChoiceNode))]
    public sealed class Angry_ChoiceNodeView: DialogueNodeView
    {
        private readonly Port Angry;
        private readonly Port Normal;

        public Angry_ChoiceNodeView(DialogueNode node, DialogueTreeView treeView): base(node, treeView)
        {
            GenerateInputPort("", true);
            Angry = GenerateOutputPort("in anger", true);
            Normal = GenerateOutputPort("normal", true);

            SaveCallback += Save;
        }

        private void Save()
        {
            if (!(node is Angry_ChoiceNode angryChoiceNode)) return;
            angryChoiceNode.Angrys = GetLinkNodes(Angry);
            angryChoiceNode.Normal = GetLinkNodes(Normal);
        }
    }
}