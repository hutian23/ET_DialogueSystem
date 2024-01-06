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
            GenerateDescription();
            Angry = this.GenerateOutputPort("in anger", true);
            Normal = this.GenerateOutputPort("normal", true);

            this.SaveCallback += this.Save;
        }

        public override void GenerateEdge()
        {
            if (!(node is Angry_ChoiceNode angryChoiceNode)) return;
            treeView.CreateEdges(Angry, angryChoiceNode.Angrys);
            treeView.CreateEdges(Normal, angryChoiceNode.Normal);
        }

        private void Save()
        {
            if (!(node is Angry_ChoiceNode angryChoiceNode)) return;
            angryChoiceNode.Angrys = GetLinkNodes(Angry);
            angryChoiceNode.Normal = GetLinkNodes(Normal);
        }

        public override DialogueNode Clone()
        {
            Angry_ChoiceNode choiceNode = node as Angry_ChoiceNode;
            choiceNode.Angrys.Clear();
            choiceNode.Normal.Clear();
            return base.Clone();
        }
    }
}