using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class CommentBlockGroup: Group
    {
        public CommentBlockData blockData;
        private readonly DialogueTreeView treeView;
        public CommentBlockGroup(CommentBlockData block,DialogueTreeView dialogueTreeView)
        {
            this.blockData = block;
            this.title = this.blockData.title;
            this.treeView = dialogueTreeView;
        }
        
        protected override void OnGroupRenamed(string oldName, string newName)
        {
            this.treeView.SetDirty();
        }
        
        public void Save()
        {
            this.blockData.title = this.title;

            this.blockData.position.x = this.GetPosition().xMin;
            this.blockData.position.y = this.GetPosition().yMin;
            var nodes = this.containedElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().Select(x => x.viewDataKey).ToList();
            this.blockData.children.Clear();
            this.blockData.children = nodes;
        }
    }
}