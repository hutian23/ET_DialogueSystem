﻿using UnityEditor.Experimental.GraphView;

namespace ET.Client
{
    [NodeEditorOf(typeof (RootNode))]
    public sealed class RootNodeView: DialogueNodeView
    {
        private readonly Port outputPort;

        public RootNodeView(DialogueNode node): base(node)
        {
            this.capabilities &= ~ Capabilities.Movable;
            this.capabilities &= ~ Capabilities.Deletable;

            this.title = "根节点";
            this.outputPort = GenerateOutputPort("开始");
        }

        public override void GenerateEdge(DialogueTreeView treeView)
        {
            if (!(this.node is RootNode rootNode)) return;
            treeView.CreateEdge(this.outputPort, rootNode.nextNode);
        }

        public override void Save(DialogueTreeView treeView)
        {
            base.Save(treeView);
            if (!(this.node is RootNode rootNode)) return;
            rootNode.nextNode = this.GetFirstLinkNode(this.outputPort);
        }
    }
}