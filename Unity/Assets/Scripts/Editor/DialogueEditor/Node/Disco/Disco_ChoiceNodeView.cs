using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ET.Client
{
    [NodeEditorOf(typeof (Disco_ChoiceNode))]
    public sealed class Disco_ChoiceNodeView: DialogueNodeView
    {
        public Port SuccessPort;
        public Port FailedPort;
        public Label label;
        
        public Disco_ChoiceNodeView(DialogueNode node): base(node)
        {
            this.title = "检定节点";
            this.inputPort = this.GenerateInputPort("", true);
            this.label = this.GenerateDescription();
            this.SuccessPort = this.GenerateOutputPort("检定成功", false);
            this.FailedPort = this.GenerateOutputPort("检定失败", false);

            this.OnNodeSelected += this.Refresh_Desc;
            this.OnNodeSelected?.Invoke(this);
        }

        private void Refresh_Desc(DialogueNodeView nodeView)
        {
            this.label.text = this.node.text;
        }
    }
}