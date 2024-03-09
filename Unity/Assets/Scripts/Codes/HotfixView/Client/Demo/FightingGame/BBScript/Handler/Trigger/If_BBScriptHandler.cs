using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public class If_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "If";
        }

        //If: HP > 10
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            SyntaxNode root = GenerateSyntaxTree(parser, data);
            Status ret = await HandleSyntaxTree(parser, data, root, token);
            RecycleSyntaxTree(root);

            if (token.IsCancel()) return Status.Failed;
            return ret;
        }

        private SyntaxNode GenerateSyntaxTree(BBParser parser, BBScriptData data)
        {
            Stack<SyntaxNode> conditionStack = new Stack<SyntaxNode>();

            int index = parser.function_Pointers[data.functionID];
            //嵌套if的根节点
            SyntaxNode rootNode = SyntaxNode.Create(SyntaxType.Condition, index);
            conditionStack.Push(rootNode);

            while (++index < parser.opDict.Count && conditionStack.Count != 0)
            {
                string opLine = parser.opDict[index];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return null;
                }

                string opType = match.Value;
                switch (opType)
                {
                    case "If":
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

        private async ETTask<Status> HandleSyntaxTree(BBParser parser, BBScriptData data, SyntaxNode node, ETCancellationToken token)
        {
            string opLine = parser.opDict[node.index];
            parser.function_Pointers[data.functionID] = node.index;

            switch (node.nodeType)
            {
                case SyntaxType.Condition:
                {
                    //条件判断 CheckHP_TriggerHandler
                    Match match = Regex.Match(opLine, @":\s*(\w+)");
                    if (!match.Success)
                    {
                        Log.Error($"not found trigger handler: {opLine}");
                        return Status.Failed;
                    }

                    BBScriptData _data = BBScriptData.Create(opLine, data.functionID);
                    bool ret = DialogueDispatcherComponent.Instance.GetTrigger(match.Groups[1].Value).Check(parser, _data);
                    //判定失败, 跳过整个if块中的代码
                    if (!ret)
                    {
                        parser.function_Pointers[data.functionID] = node.endIndex;
                        return Status.Success;
                    }

                    break;
                }
                case SyntaxType.Normal:
                {
                    //匹配OpType
                    Match match2 = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                    if (!match2.Success)
                    {
                        Log.Error($"not found bbScriptHandler: {opLine}");
                        return Status.Failed;
                    }

                    if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(match2.Value, out BBScriptHandler handler))
                    {
                        Log.Error($"not found script handler: {match2.Value}");
                        return Status.Failed;
                    }

                    BBScriptData _data = BBScriptData.Create(opLine, data.functionID);
                    Status ret = await handler.Handle(parser, _data, token);

                    if (token.IsCancel()) return Status.Failed;
                    if (ret != Status.Success) return ret;
                    break;
                }
            }

            //递归执行子节点
            foreach (SyntaxNode n in node.children)
            {
                Status ret = await HandleSyntaxTree(parser, data, n, token);
                if (ret != Status.Success) return ret; //子节点执行失败，停止递归
            }

            //指针跳过EndIf
            if (node.nodeType == SyntaxType.Condition) parser.function_Pointers[data.functionID] = node.endIndex;
            return Status.Success;
        }

        /// <summary>
        /// 回收语法树
        /// </summary>
        private void RecycleSyntaxTree(SyntaxNode root)
        {
            Stack<SyntaxNode> stack = new();
            RecycleNode(stack, root);
            while (stack.Count != 0)
            {
                stack.Pop().Recycle();
            }
        }

        private void RecycleNode(Stack<SyntaxNode> stack, SyntaxNode node)
        {
            stack.Push(node);
            node.children.ForEach(child => { RecycleNode(stack, child); });
        }
    }
}