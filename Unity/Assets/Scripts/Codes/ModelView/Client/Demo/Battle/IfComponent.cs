using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class IfComponent : Entity, IAwake, IDestroy
    {
        public SyntaxNode Root;
        
        public int startIndex;
        public int endIndex;
        public int curIndex;
    }
    
    #region If
    public enum SyntaxType
    {
        None,
        Condition,
        Normal
    }

    [Serializable]
    public class SyntaxNode
    {
        public List<SyntaxNode> children = new();
        public SyntaxType nodeType;
        public int index;
        public int endIndex;

        public static SyntaxNode Create(SyntaxType nodeType, int index)
        {
            SyntaxNode node = ObjectPool.Instance.Fetch<SyntaxNode>();
            node.nodeType = nodeType;
            node.index = index;
            return node;
        }

        public void Recycle()
        {
            this.nodeType = SyntaxType.None;
            this.index = 0;
            this.endIndex = 0;
            this.children.Clear();
            ObjectPool.Instance.Recycle(this);
        }
    }
    #endregion
}