using UnityEditor.Experimental.GraphView;
namespace ET.Client
{
    [NodeEditorOf(typeof (InterrogateNode))]
    public class InterrogateNodeView: DialogueNodeView
    {
        private readonly Port holditPort, successPort, failedPort, nextsPort;

        public InterrogateNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            nextsPort = GenerateOutputPort("下一个: ", true);
            holditPort = GenerateOutputPort("等等: ");
            successPort = GenerateOutputPort("出示(检定成功): ");
            failedPort = GenerateOutputPort("出示(检定失败): ");
            
            this.SaveCallback += Save;
        }

        private void Save()
        {
            if (node is not InterrogateNode interrogateNode) return;
            interrogateNode.hold_it = GetFirstLinkNode(holditPort);
            interrogateNode.take_that_Success = GetFirstLinkNode(successPort);
            interrogateNode.take_that_Failed = GetFirstLinkNode(failedPort);
            interrogateNode.nexts = GetLinkNodes(nextsPort);
        }
    }
}