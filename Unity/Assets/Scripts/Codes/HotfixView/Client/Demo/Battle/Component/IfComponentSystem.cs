using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(IfComponent))]
    [FriendOf(typeof(BBParser))]
    public static class IfComponentSystem
    {
        public class IfComponentDestroySystem : DestroySystem<IfComponent>
        {
            protected override void Destroy(IfComponent self)
            {
                self.startIndex = 0;
                self.curIndex = 0;
                self.endIndex = 0;
                self.RecycleSyntaxTree();
            }
        }
        
        private static SyntaxNode GenerateSyntaxNode(this IfComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();

            //1. 生成Root节点
            Stack<SyntaxNode> conditionStack = new Stack<SyntaxNode>();
            SyntaxNode rootNode = SyntaxNode.Create(SyntaxType.Condition, self.startIndex);
            conditionStack.Push(rootNode);
            
            //2. 生成子节点
            int index = self.startIndex;
            while (++index < parser.OpDict.Count && conditionStack.Count != 0)
            {
                string opLine = parser.OpDict[index];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    ScriptHelper.ScripMatchError(opLine);
                    return null;
                }
                
                switch (match.Value)
                {
                    // If作为父节点
                    case "BeginIf":
                        SyntaxNode parent = SyntaxNode.Create(SyntaxType.Condition, index);
                        conditionStack.Peek().children.Add(parent);
                        conditionStack.Push(parent);
                        break;
                    // 结束If代码块
                    case "EndIf":
                        SyntaxNode conditionNode = conditionStack.Pop();
                        conditionNode.endIndex = index;
                        break;
                    // Action为子节点
                    default:
                        SyntaxNode normal = SyntaxNode.Create(SyntaxType.Normal, index);
                        conditionStack.Peek().children.Add(normal);
                        break;
                }
            }
            
            return rootNode;
        }

        public static async ETTask<Status> IfCor(this IfComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();
            
            //1. 生成If协程Id
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            parser.Coroutine_Pointers.Add(funcId, self.startIndex);
            
            //2. 取消行为协程时，取消If协程
            parser.CancellationToken.Add(() => { parser.Coroutine_Pointers.Remove(funcId); });
            
            //3. 执行If协程
            Status ret = await self.HandleSyntaxNode(self.Root, funcId);
            return ret;
        }
        
        private static async ETTask<Status> HandleSyntaxNode(this IfComponent self, SyntaxNode node, long funcId)
        {
            BBParser parser = self.GetParent<BBParser>();
            
            //1. 更新指针位置
            parser.Coroutine_Pointers[funcId] = node.index;
            self.curIndex = node.index;
            string opLine = parser.OpDict[self.curIndex];
            
            //2. 执行节点
            switch (node.nodeType)
            {
                // Condition节点
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

                        BBScriptData _data = BBScriptData.Create(op, funcId);
                        bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(parser, _data);
                        _data.Recycle();

                        // 判定失败，跳过整个if块
                        if (!ret) return Status.Success;
                    }
                    break;
                }

                // Normal节点
                case SyntaxType.Normal:
                {
                    Match match2 = Regex.Match(opLine,@"^\w+\b(?:\(\))?");
                    if (!match2.Success)
                    {
                        Log.Error($"not found bbScriptHandler: {opLine}");
                        return Status.Failed;
                    }
                    
                    BBScriptData _data = BBScriptData.Create(opLine, funcId);
                    Status ret = await ScriptDispatcherComponent.Instance.GetScriptHandler(match2.Value).Handle(parser, _data, parser.CancellationToken);
                    _data.Recycle();

                    if (parser.CancellationToken.IsCancel()) return Status.Failed;
                    if (ret != Status.Success) return ret;
                    break;
                }
            }
            
            //3. 递归执行子节点
            foreach (SyntaxNode n in node.children)
            {
                Status ret = await self.HandleSyntaxNode(n, funcId);
                if (ret != Status.Success) return ret; // 子节点执行失败，停止递归
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
            node.children.ForEach(child => { self.RecycleSyntaxNode(stack, child); });
        }
    }
}