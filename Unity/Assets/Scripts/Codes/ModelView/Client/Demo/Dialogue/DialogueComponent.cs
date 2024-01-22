using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class DialogueComponent: Entity, IAwake, IDestroy, ILoad
    {
        public ETCancellationToken token;

        public Dictionary<uint, DialogueNode> targets = new();
        public Queue<DialogueNode> workQueue = new();
    }
}