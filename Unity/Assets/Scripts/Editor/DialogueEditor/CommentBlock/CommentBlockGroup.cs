using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class CommentBlockGroup : Group
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

        public override bool AcceptsElement(GraphElement element, ref string reasonWhyNotAccepted)
        {
            if (element is DialogueNodeView nodeView)
            {
                this.blockData.children.Add(nodeView.GetNodeGuid());
            }

            this.desc.text = this.blockData.children.Count.ToString();
            return base.AcceptsElement(element, ref reasonWhyNotAccepted);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            this.blockData.position.x = newPos.xMin;
            this.blockData.position.y = newPos.yMin;
        }
    }
}