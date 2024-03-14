using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(DialogueComponent))]
    public class WhiffCancel : Entity,IAwake,IDestroy
    {
        public HashSet<string> cancelTags = new();
        public ETCancellationToken token;
    }
}