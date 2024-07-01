using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    public class If_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "BeginIf";
        }

        //BeginIf HP > 10:
        //  LogWarning: 'Hello world';
        //  BeginIf:
        //      LogWarning: 'Hello world';
        //      LogWarning: '2222';
        //  EndIf:
        //  LogWarning: '22222';
        //  LogWarning: '222333';
        //EndIf:
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            ScriptParser parser = unit.GetComponent<TimelineComponent>().GetComponent<ScriptParser>();

            BBSyntaxNode rootNode = GenerateSyntaxTree(parser, data);
            await HandleSyntaxTree(parser, data, rootNode);

            await ETTask.CompletedTask;
            return Status.Success;
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

                switch (opType)
                {
                    case "BeginIf":
                        BBSyntaxNode child = BBSyntaxNode.Create(pointer);
                        conditionStack.Peek().children.Add(child);
                        conditionStack.Push(child);
                        break;
                    case "EndIf":
                        conditionStack.Pop();
                        break;
                    default:
                        BBSyntaxNode normal = BBSyntaxNode.Create(pointer);
                        conditionStack.Peek().children.Add(normal);
                        break;
                }
            }

            return rootNode;
        }

        private async ETTask<Status> HandleSyntaxTree(ScriptParser parser, ScriptData data, BBSyntaxNode rootNode)
        {
            BBTimerComponent timerComponent = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            Queue<BBSyntaxNode> workQueue = new Queue<BBSyntaxNode>();
            workQueue.Enqueue(rootNode);

            while (workQueue.Count > 0)
            {
                BBSyntaxNode node = workQueue.Dequeue();
                Log.Warning(parser.opDict[node.index]);

                foreach (var child in node.children)
                {
                    workQueue.Enqueue(child);
                }
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}