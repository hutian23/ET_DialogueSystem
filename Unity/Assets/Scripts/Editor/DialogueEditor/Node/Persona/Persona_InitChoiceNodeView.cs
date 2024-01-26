using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    public sealed class Persona_InitChoiceNodeView: DialogueNodeView<Persona_InitChoiceNode>
    {
        private readonly Port charaPort;
        private readonly Port moneyPort;
        private readonly Port itemPort;
        private readonly Port extraPort;

        public Persona_InitChoiceNodeView(Persona_InitChoiceNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode,
            dialogueTreeView)
        {
            GenerateInputPort("");
            charaPort = GenerateOutputPort("人格面具", true);
            moneyPort = GenerateOutputPort("要钱", true);
            itemPort = GenerateOutputPort("要道具", true);
            extraPort = GenerateOutputPort("特殊", true);
            SaveCallback += Save;
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