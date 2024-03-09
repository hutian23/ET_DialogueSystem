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
            await ETTask.CompletedTask;
            //匹配条件码 HP > 10
            Match match = Regex.Match(data.opLine, @"If:\s*(.*)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //条件判断 CheckHP_TriggerHandler
            Match match2 = Regex.Match(match.Groups[1].Value, @"^\w+");
            if (!match2.Success)
            {
                Log.Error($"not found trigger handler: {match.Groups[1].Value}");
                return Status.Failed;
            }

            SyntaxNode root = GenerateSyntaxTree(parser, data);
            await HandleSyntaxTree(parser, data, root, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }

        private SyntaxNode GenerateSyntaxTree(BBParser parser, BBScriptData data)
        {
            Stack<SyntaxNode> conditionStack = new Stack<SyntaxNode>();

            int index = parser.function_Pointers[data.functionID];
            //嵌套if的根节点
            SyntaxNode rootNode = new() { nodeType = SyntaxType.Condition, index = index };
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
                        SyntaxNode child = new() { nodeType = SyntaxType.Condition, index = index };
                        conditionStack.Peek().children.Add(child);
                        conditionStack.Push(child);
                        break;
                    case "EndIf":
                        conditionStack.Pop();
                        break;
                    default:
                        SyntaxNode child_normal = new() { nodeType = SyntaxType.Normal, index = index };
                        conditionStack.Peek().children.Add(child_normal);
                        break;
                }
            }

            return rootNode;
        }

        private async ETTask HandleSyntaxTree(BBParser parser, BBScriptData data, SyntaxNode node, ETCancellationToken token)
        {
            string opLine = parser.opDict[node.index];
            parser.function_Pointers[data.functionID] = node.index;
            Log.Warning(opLine);

            await TimerComponent.Instance.WaitFrameAsync(token);
            if (token.IsCancel()) return;

            foreach (SyntaxNode n in node.children)
            {
                await HandleSyntaxTree(parser, data, n, token);
            }

            if (node.nodeType == SyntaxType.Condition) parser.function_Pointers[data.functionID]++;
        }
    }
}