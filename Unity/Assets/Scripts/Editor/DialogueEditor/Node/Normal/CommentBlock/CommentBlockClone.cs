using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ET.Client
{
    public class CommentBlockClone
    {
        private readonly Vector2 OFFSET = new(100, 100);

        //不能跨对话树进行复制
        // private readonly CommentBlockGroup blockGroup;
        //
        // public CommentBlockClone(CommentBlockGroup group)
        // {
        //     this.blockGroup = group;
        // }

        public CommentBlockData blockData;
        public List<DialogueNode> nodes = new();
        public List<NodeLinkData> linkDatas = new();

        public CommentBlockClone(CommentBlockGroup group)
        {
            //保存背景板
            blockData = MongoHelper.Clone(group.blockData);
            blockData.children.Clear();
            blockData.position += OFFSET;

            var nodeCaches = group.containedElements.OfType<DialogueNodeView>().ToList();
            //保存连线
            nodeCaches.ForEach(nodeCache =>
            {
                //深拷贝节点
                DialogueNode cloneNode = MongoHelper.Clone(nodeCache.node);
                cloneNode.position += OFFSET;
                nodes.Add(cloneNode);

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
            //新旧节点guid映射
            var nodeCacheDict = new Dictionary<string, string>();
            var cloneNodes = new List<DialogueNode>();
            this.nodes.ForEach(dialogueNode =>
            {
                DialogueNode cloneNode = MongoHelper.Clone(dialogueNode);
                cloneNode.position += OFFSET;
                cloneNode.TargetID = 0;
                cloneNode.Guid = GUID.Generate().ToString();

                nodeCacheDict.Add(dialogueNode.Guid, cloneNode.Guid);
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