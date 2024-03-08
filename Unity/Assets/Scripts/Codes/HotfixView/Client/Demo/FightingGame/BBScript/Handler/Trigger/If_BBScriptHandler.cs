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

            TransformToSyntaxTree(parser, data, out SyntaxNode node);
            foreach (string opline in node.oplines)
            {
                if (token.IsCancel()) return Status.Failed;
                Match match3 = Regex.Match(opline, @"^\w+\b(?:\(\))?");
                if (!match3.Success)
                {
                    DialogueHelper.ScripMatchError(opline);
                    return Status.Failed;
                }

                string opType = match3.Value;
                if (opType == "SetMarker")
                {
                    Log.Error($"cannot use marker in if-block!!!");
                    return Status.Failed;
                }

                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                BBScriptData subData = BBScriptData.Create(opline, data.functionID); //池化，不然GC很高
                Status ret = await handler.Handle(parser, subData, token); //执行一条语句相当于一个子协程
                data.Recycle();
                if (ret == Status.Return) return Status.Success;
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
            }

            parser.function_Pointers[data.functionID] = ++node.endIndex;
            return Status.Success;
        }

        private static void TransformToSyntaxTree(BBParser parser, BBScriptData data, out SyntaxNode rootNode)
        {
            rootNode = new SyntaxNode();
            SyntaxNode currentNode = rootNode;

            string[] opLines = parser.opLines.Split('\n');
            //If块的起始索引
            int index = parser.function_Pointers[data.functionID];
            currentNode.condition = opLines[index];
            currentNode.startIndex = index;

            while (++index < opLines.Length)
            {
                string opLine = opLines[index];
                if (string.IsNullOrEmpty(opLine) || opLine.StartsWith('#')) continue;

                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                switch (match.Value)
                {
                    case "EndIf":
                        currentNode.endIndex = index;
                        return;
                }

                currentNode.oplines.Add(opLine.Trim());
            }
        }
    }
}