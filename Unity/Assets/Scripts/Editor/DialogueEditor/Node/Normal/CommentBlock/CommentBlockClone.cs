using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ET.Client
{
    public class GroupNodeData
    {
        public DialogueNode node;
        public Vector2 localPosition;
    }

    public class CommentBlockClone
    {
        private readonly CommentBlockData blockData;
        private readonly List<GroupNodeData> nodes = new();
        private readonly List<NodeLinkData> linkDatas = new();

        public CommentBlockClone(CommentBlockGroup group)
        {
            //保存背景板
            blockData = MongoHelper.Clone(group.blockData);
            blockData.children.Clear();

            var nodeCaches = group.containedElements.OfType<DialogueNodeView>().ToList();
            //保存连线
            nodeCaches.ForEach(nodeCache =>
            {
                //深拷贝节点
                DialogueNode cloneNode = MongoHelper.Clone(nodeCache.node);
                nodes.Add(new GroupNodeData() { node = cloneNode, localPosition = cloneNode.position - blockData.position });

                for (int i = 0; i < nodeCache.outports.Count; i++)
                {
                    Port output = nodeCache.outports[i];
                    foreach (var edge in output.connections)
                    {
                        DialogueNodeView childView = edge.input.node as DialogueNodeView;
                        if (childView == null) continue;
                        //组内的节点，则保存连线
                        if (nodeCaches.Contains(childView))
                        {
                            linkDatas.Add(new NodeLinkData()
                            {
                                //ViewDataKey等于 node.Guid
                                inputNodeGuid = childView.viewDataKey, outputNodeGuid = nodeCache.viewDataKey, portID = i
                            });
                        }
                    }
                }
            });
        }

        public void Clone(DialogueTreeView treeView)
        {
            CommentBlockData cloneBlockData = MongoHelper.Clone(this.blockData);
            cloneBlockData.position = treeView.LocalMousePosition;

            //新旧节点guid映射
            var nodeCacheDict = new Dictionary<string, string>();
            var cloneNodes = new List<DialogueNode>();
            this.nodes.ForEach(groupNode =>
            {
                DialogueNode cloneNode = MongoHelper.Clone(groupNode.node);
                cloneNode.TargetID = 0;
                cloneNode.Guid = GUID.Generate().ToString();
                cloneNode.position = cloneBlockData.position + groupNode.localPosition;

                nodeCacheDict.Add(groupNode.node.Guid, cloneNode.Guid);
                cloneNodes.Add(cloneNode);
            });

            var cloneLinkDatas = new List<NodeLinkData>();
            this.linkDatas.ForEach(linkData => { cloneLinkDatas.Add(MongoHelper.Clone(linkData)); });
            //替换guid
            cloneLinkDatas.ForEach(linkData =>
            {
                nodeCacheDict.TryGetValue(linkData.inputNodeGuid, out string newInputNodeGuid);
                nodeCacheDict.TryGetValue(linkData.outputNodeGuid, out string newOutputNodeGuid);
                linkData.inputNodeGuid = newInputNodeGuid;
                linkData.outputNodeGuid = newOutputNodeGuid;
            });

            cloneBlockData.children.Clear();
            cloneBlockData.children = cloneNodes.Select(x => x.Guid).ToList();

            //1. 克隆节点
            cloneNodes.ForEach(treeView.CreateNode);
            //2. 生成边
            cloneLinkDatas.ForEach(treeView.CreateEdge);
            //3. 生成commentblock
            treeView.CreateCommentBlock(cloneBlockData);
        }
    }
}