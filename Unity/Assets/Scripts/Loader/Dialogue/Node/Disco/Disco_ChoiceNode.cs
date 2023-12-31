namespace ET
{
    [NodeType("Disco/检定节点")]
    public class Disco_ChoiceNode: DialogueNode
    {
        //检定成功
        public DialogueNode Success;

        //检定失败
        public DialogueNode Failed;
    }
}