using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Persona_moneyNode))]
    public class Persona_moneyNodeView: DialogueNodeView
    {
        private readonly Port success;
        private readonly Port failed;

        public Persona_moneyNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("",true);
            success = this.GenerateOutputPort("检定成功");
            failed = this.GenerateOutputPort("检定失败");
            this.GenerateDescription();
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if (node is not Persona_moneyNode moneyNode) return;
            moneyNode.Failed = this.GetLinkNodes(failed);
            moneyNode.Success = this.GetLinkNodes(success);
        }
    }
}