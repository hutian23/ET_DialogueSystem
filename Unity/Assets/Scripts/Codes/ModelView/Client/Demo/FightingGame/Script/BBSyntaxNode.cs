using System;
using System.Collections.Generic;

namespace ET.Client
{
    [Serializable]
    public class BBSyntaxNode
    {
        public List<BBSyntaxNode> children = new();
        
        public int startIndex; //当前代码块的起始索引
        public int endIndex; //结束索引
        
        public static BBSyntaxNode Create(int _index)
        {
            BBSyntaxNode syntaxNode = ObjectPool.Instance.Fetch<BBSyntaxNode>();
            syntaxNode.startIndex = _index;
            syntaxNode.endIndex = _index;
            return syntaxNode;
        }

        public void Recycle()
        {
            startIndex = 0;
            endIndex = 0;
            children.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
}