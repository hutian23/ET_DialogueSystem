namespace ET
{
    [NodeType("Disco/ChoiceNode","分支节点")]
    public class ChoiceNode: DialogueNode
    {
        public string choice1;
    }
    
    [NodeType("Disco/ChoiceNode","分支节点2")]
    public class ChoiceNode2: DialogueNode
    {
        public string choice1;
    }

    [NodeType("Disco/Test1/Test2","测试节点")]
    public class TestNode: DialogueNode
    {
        
    }
}