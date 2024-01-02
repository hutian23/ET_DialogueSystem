using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Angry_ChoiceNode))]
    public sealed class Angry_ChoiceNodeView: DialogueNodeView
    {
        private readonly Port Angry;
        private readonly Port Normal;

        public Angry_ChoiceNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            title = "情绪分支节点(Katana zero)";

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
    }
}