using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Disco_ChoiceNode))]
    public sealed class Disco_ChoiceNodeView: DialogueNodeView
    {
        private readonly Port SuccessPort;
        private readonly Port FailedPort;
        
        public Disco_ChoiceNodeView(DialogueNode node): base(node)
        {
            this.title = "检定节点(Disco)";
            
            this.GenerateInputPort("", true);
            this.GenerateDescription();
            this.SuccessPort = this.GenerateOutputPort("检定成功");
            this.FailedPort = this.GenerateOutputPort("检定失败");

            this.SaveCallback += this.Save;
        }
        

        public override void GenerateEdge(DialogueTreeView treeView)
        {
            if (!(this.node is Disco_ChoiceNode choiceNode)) return;
            treeView.CreateEdge(this.SuccessPort, choiceNode.Success);
            treeView.CreateEdge(this.FailedPort, choiceNode.Failed);
        }

        private void Save(DialogueTreeView treeView)
        {
            if (!(this.node is Disco_ChoiceNode choiceNode)) return;
            choiceNode.Success = this.GetFirstLinkNode(this.SuccessPort);
            choiceNode.Failed = this.GetFirstLinkNode(this.FailedPort);
        }
    }
}