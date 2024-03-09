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
            await HandleSyntaxTree(parser, root, token);
            if (token.IsCancel()) return Status.Failed;

            root.children.ForEach(n => { parser.function_Pointers[data.functionID] = n.index; });
            parser.function_Pointers[data.functionID]++;
            return Status.Success;
        }

        private SyntaxNode GenerateSyntaxTree(BBParser parser, BBScriptData data)
        {
            Stack<SyntaxNode> conditionStack = new Stack<SyntaxNode>();

            var opLines = parser.opLines.Split('\n');
            int index = parser.function_Pointers[data.functionID];
            //嵌套if的根节点
            SyntaxNode rootNode = new() { nodeType = SyntaxType.Condition, index = index };
            conditionStack.Push(rootNode);

            while (++index < opLines.Length && conditionStack.Count != 0)
            {
                if (string.IsNullOrEmpty(opLines[index]) || opLines[index].StartsWith('#')) continue;

                string opLine = opLines[index].Trim();
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

        private async ETTask HandleSyntaxTree(BBParser parser, SyntaxNode node, ETCancellationToken token)
        {
            var opLines = parser.opLines.Split('\n');
            Log.Warning(opLines[node.index].Trim());

            await TimerComponent.Instance.WaitFrameAsync(token);
            if (token.IsCancel()) return;

            foreach (var n in node.children)
            {
                await HandleSyntaxTree(parser, n, token);
            }
        }
    }
}