using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class CommentBlockGroup: Group
    {
        public CommentBlockData blockData;

        private Label desc;

        public CommentBlockGroup(CommentBlockData block)
        {
            this.blockData = block;
            this.title = this.blockData.title;
            this.desc = new Label();
            this.contentContainer.Add(this.desc);
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            this.blockData.title = newName;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            this.blockData.position.x = newPos.xMin;
            this.blockData.position.y = newPos.yMin;
        }

        public void Save()
        {
            var nodes = this.containedElements.Where(x => x is DialogueNodeView).Cast<DialogueNodeView>().Select(x => x.viewDataKey).ToList();
            this.blockData.children.Clear();
            this.blockData.children = nodes;
        }
    }
}