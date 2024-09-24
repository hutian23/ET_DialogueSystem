using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public class If_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "BeginIf";
        }

        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            BBSyntaxNode rootNode = GenerateSyntaxTree(parser, data);
            return await HandleSyntaxTree(parser, data, rootNode);
        }

        private BBSyntaxNode GenerateSyntaxTree(ScriptParser parser, ScriptData data)
        {
            Stack<BBSyntaxNode> conditionStack = new Stack<BBSyntaxNode>();

            //1. find current pointer
            parser.subCoroutineDatas.TryGetValue(data.coroutineID, out SubCoroutineData coroutineData);
            int pointer = coroutineData.pointer;

            //2. enqueue rootnode
            BBSyntaxNode rootNode = BBSyntaxNode.Create(pointer);
            conditionStack.Push(rootNode);

            while (++pointer < parser.opDict.Count && conditionStack.Count != 0)
            {
                //find opType
                string opLine = parser.opDict[pointer];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    ScriptHelper.ScriptMatchError(opLine);
                    return null;
                }

                string opType = match.Value;

                BBSyntaxNode child = BBSyntaxNode.Create(pointer);
                switch (opType)
                {
                    case "BeginIf":
                        conditionStack.Peek().children.Add(child);
                        conditionStack.Push(child);
                        break;
                    case "EndIf":
                        conditionStack.Peek().endIndex = pointer;
                        conditionStack.Peek().children.Add(child);
                        conditionStack.Pop();
                        break;
                    default:
                        conditionStack.Peek().children.Add(child);
                        break;
                }
            }

            return rootNode;
        }

        private async ETTask<Status> HandleSyntaxTree(ScriptParser parser, ScriptData data, BBSyntaxNode syntaxNode)
        {
            //1. get subcoroutine
            parser.subCoroutineDatas.TryGetValue(data.coroutineID, out SubCoroutineData coroutineData);
            if (coroutineData == null)
            {
                Log.Error($"not found coroutineData: {data.coroutineID}");
                return Status.Failed;
            }

            //2. get OpType
            parser.ReplaceParam(parser.opDict[syntaxNode.startIndex], out string opLine);
            Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(opLine);
                return Status.Failed;
            }

            string opType = match.Value;

            switch (opType)
            {
                case "BeginIf":
                {
                    Match match2 = Regex.Match(opLine, "BeginIf: (.+)");
                    string triggerLine = match2.Groups[1].Value;
                    string triggerType = triggerLine.Split(' ')[0];

                    ScriptDispatcherComponent.Instance.TriggerHandlers.TryGetValue(triggerType, out TriggerHandler triggerHandler);
                    ScriptData scriptData = ScriptData.Create(triggerLine, coroutineData.coroutineName);

                    //条件不符合, 跳过当前if块
                    if (!triggerHandler.Check(parser, scriptData))
                    {
                        coroutineData.pointer = syntaxNode.endIndex;
                        return Status.Success;
                    }

                    break;
                }
                case "EndIf":
                {
                    break;
                }
                default:
                {
                    Match match3 = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                    if (!match3.Success)
                    {
                        Log.Error($"not found opType: {opLine}");
                        return Status.Failed;
                    }

                    if (!ScriptDispatcherComponent.Instance.ScriptHandlers.TryGetValue(match3.Value, out ScriptHandler scriptHandler))
                    {
                        Log.Error($"not found script handler: {match3.Value}");
                        return Status.Failed;
                    }

                    ScriptData _data = ScriptData.Create(opLine, coroutineData.coroutineName);
                    Status ret = await scriptHandler.Handle(parser, _data, coroutineData.token);
                    if (ret != Status.Success)
                    {
                        Log.Warning($"index: {syntaxNode.startIndex} {opLine} failed to execute");
                        return ret;
                    }

                    break;
                }
            }

            coroutineData.pointer = syntaxNode.startIndex;

            foreach (var child in syntaxNode.children)
            {
                if (coroutineData.token.IsCancel())
                {
                    return Status.Failed;
                }

                Status status = await HandleSyntaxTree(parser, data, child);
                if (status != Status.Success)
                {
                    return Status.Success;
                }
            }

            return Status.Success;
        }
    }
}