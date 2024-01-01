using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Angry_ChoiceNode))]
    public sealed class Angry_ChoiceNodeView: DialogueNodeView
    {
        private readonly Port Angry;
        private readonly Port Normal;

        public Angry_ChoiceNodeView(DialogueNode node): base(node)
        {
            this.title = "武士零愤怒节点";

            this.GenerateInputPort("", true);
            this.GenerateDescription();
            this.Angry = this.GenerateOutputPort("in anger", true);
            this.Normal = this.GenerateOutputPort("normal", true);
        }
        public override void GenerateEdge(DialogueTreeView treeView)
        {
            if (!(this.node is Angry_ChoiceNode angryChoiceNode)) return;
            treeView.CreateEdges(this.Angry, angryChoiceNode.Angrys);
            treeView.CreateEdges(this.Angry, angryChoiceNode.Normal);
        }

        public override void Save(DialogueTreeView treeView)
        {
            base.Save(treeView);
            if (!(this.node is Angry_ChoiceNode angryChoiceNode)) return;
            angryChoiceNode.Angrys = this.GetLinkNodes(this.Angry);
            angryChoiceNode.Normal = this.GetLinkNodes(this.Normal);
        }
    }
}