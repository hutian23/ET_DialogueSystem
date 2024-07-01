using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Serializable]
    public class BBSyntaxNode
    {
        public List<BBSyntaxNode> children = new();

        public int index;

        public static BBSyntaxNode Create(int _index)
        {
            BBSyntaxNode syntaxNode = ObjectPool.Instance.Fetch<BBSyntaxNode>();
            syntaxNode.index = _index;
            return syntaxNode;
        }

        public void Recycle()
        {
            index = 0;
            children.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
}