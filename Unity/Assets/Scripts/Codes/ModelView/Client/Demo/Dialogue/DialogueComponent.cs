﻿using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class DialogueComponent: Entity, IAwake, IDestroy, ILoad
    {
        public ETCancellationToken token;
        public Queue<DialogueNode> workQueue = new();

        public DialogueTreeData treeData;
        public int ReloadType;

        public HashSet<int> tags = new();
    }

    public static class DialogueTag
    {
        public const int None = 0;
        public const int Typing = 1;
    }
}