using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(IfComponent))]
    [FriendOf(typeof(BBParser))]
    public static class IfComponentSystem
    {
        public class IfComponentAwakeSystem : AwakeSystem<IfComponent, int, int>
        {
            protected override void Awake(IfComponent self, int startIndex, int endIndex)
            {
                self.startIndex = startIndex;
                self.endIndex = endIndex;
                self.Root = self.GenerateSyntaxNode();
                self.token = new ETCancellationToken();
            }
        }
        
        public class IfComponentDestroySystem : DestroySystem<IfComponent>
        {
            protected override void Destroy(IfComponent self)
            {
                self.startIndex = 0;
                self.endIndex = 0;
                self.RecycleSyntaxTree();
                self.token.Cancel();
            }
        }
        
        private static SyntaxNode GenerateSyntaxNode(this IfComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();

            Stack<SyntaxNode> conditionStack = new Stack<SyntaxNode>();
            SyntaxNode rootNode = SyntaxNode.Create(SyntaxType.Condition, self.startIndex);
            conditionStack.Push(rootNode);
            
            int index = self.startIndex;
            while (++index < self.endIndex)
            {
                string opLine = parser.OpDict[index];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    ScriptHelper.ScripMatchError(opLine);
                    return null;
                }
                
                string opType = match.Value;
                switch (opType)
                {
                    case "BeginIf":
                        SyntaxNode child = SyntaxNode.Create(SyntaxType.Condition, index);
                        conditionStack.Peek().children.Add(child);
                        conditionStack.Push(child);
                        break;
                    case "EndIf":
                        SyntaxNode conditionNode = conditionStack.Pop();
                        conditionNode.endIndex = index;
                        break;
                    default:
                        SyntaxNode child_normal = SyntaxNode.Create(SyntaxType.Normal, index);
                        conditionStack.Peek().children.Add(child_normal);
                        break;
                }
            }

            return rootNode;
        }

        public static async ETTask<Status> HandleSyntaxTree(this IfComponent self, SyntaxNode node)
        {
            BBParser parser = self.GetParent<BBParser>();

            string opLine = parser.OpDict[node.index];

            switch (node.nodeType)
            {
                case SyntaxType.Condition:
                {
                    // BeginIf: (InputType: RunHold), (MoveType: Special),....()
                    MatchCollection matches = Regex.Matches(opLine, @"\((.*?)\)");
                    // 没有条件判断(语法错误)
                    if (matches.Count == 0)
                    {
                        Log.Error($"BeginIf_Handler must have at least one triggerHandler!");
                        return Status.Failed;
                    }
                    
                    // 逐个判断条件
                    foreach (Match match in matches)
                    {
                        string op = match.Groups[1].Value;
                        Match triggerMatch = Regex.Match(op, @"(.*?):");
                        if (!triggerMatch.Success)
                        {
                            ScriptHelper.ScripMatchError(op);
                            return Status.Failed;
                        }

                        BBScriptData _data = BBScriptData.Create(op, self.startIndex);
                        // 判定失败，跳过整个if块中的代码
                        bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(parser, _data);
                    }
                }
                    break;
            }
            return Status.Success;
        }

        private static void RecycleSyntaxTree(this IfComponent self)
        {
            Stack<SyntaxNode> stack = new();
            self.RecycleSyntaxNode(stack, self.Root);
            while (stack.Count != 0)
            {
                stack.Pop().Recycle();
            }
        }

        private static void RecycleSyntaxNode(this IfComponent self, Stack<SyntaxNode> stack, SyntaxNode node)
        {
            stack.Push(node);
            node.children.ForEach(child => {self.RecycleSyntaxNode(stack, child);});
        }
    }
}