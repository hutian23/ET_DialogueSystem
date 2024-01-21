using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Disco_ChoiceNode))]
    public sealed class Disco_ChoiceNodeView: DialogueNodeView
    {
        private readonly Port SuccessPort;
        private readonly Port FailedPort;
        
        public Disco_ChoiceNodeView(DialogueNode node,DialogueTreeView treeView): base(node,treeView)
        {
            GenerateInputPort("", true);
            SuccessPort = GenerateOutputPort("检定成功");
            FailedPort = GenerateOutputPort("检定失败");

            SaveCallback += Save;
        }
        
        private void Save()
        {
            if (!(node is Disco_ChoiceNode choiceNode)) return;
            choiceNode.Success = GetFirstLinkNode(SuccessPort);
            choiceNode.Failed = GetFirstLinkNode(FailedPort);
        }
    }
}