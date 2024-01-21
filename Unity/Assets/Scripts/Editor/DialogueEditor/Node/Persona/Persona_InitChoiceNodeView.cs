using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (Persona_InitChoiceNode))]
    public class Persona_InitChoiceNodeView: DialogueNodeView
    {
        private readonly Port charaPort;
        private readonly Port moneyPort;
        private readonly Port itemPort;
        private readonly Port extraPort;
        public Persona_InitChoiceNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            this.GenerateInputPort("");
            charaPort = this.GenerateOutputPort("人格面具", true);
            moneyPort = this.GenerateOutputPort("要钱", true);
            itemPort = this.GenerateOutputPort("要道具", true);
            extraPort = this.GenerateOutputPort("特殊", true);
            this.SaveCallback += this.Save;
        }

        private void Save()
        {
            if (!(node is Persona_InitChoiceNode initNode)) return;
            initNode.Character = GetLinkNodes(charaPort);
            initNode.Money = GetLinkNodes(moneyPort);
            initNode.Item = GetLinkNodes(itemPort);
            initNode.extras = GetLinkNodes(extraPort);
        }
    }
}