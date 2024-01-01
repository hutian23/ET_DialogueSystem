using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class NodeEditorOfAttribute: Attribute
    {
        public Type nodeType;

        public NodeEditorOfAttribute(Type type)
        {
            this.nodeType = type;
        }
    }

    public abstract class DialogueNodeView: Node
    {
        public DialogueNode node;

        public Action<DialogueNodeView> OnNodeSelected;
        public Port input;
        public Dictionary<string, Port> outports = new();
        public Label desc;

        protected DialogueNodeView(DialogueNode node)
        {
            this.node = node;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/Node/NodeView.uss"));
            this.node = node;
            this.viewDataKey = this.node.Guid;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            this.node.position.x = newPos.xMin;
            this.node.position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (this.OnNodeSelected != null)
            {
                this.OnNodeSelected.Invoke(this);
            }
        }

        protected Label GenerateDescription()
        {
            this.desc = new();
            this.desc.enableRichText = true;
            this.desc.AddToClassList("input-port-label");

            this.inputContainer.Add(desc);
            this.inputContainer.style.flexDirection = FlexDirection.Row;

            this.OnNodeSelected += this.RefreshDesc;
            this.RefreshDesc(this);
            return desc;
        }

        private void RefreshDesc(DialogueNodeView nodeView)
        {
            nodeView.desc.text = this.node.text;
        }

        /// <summary>
        /// 任何nodeview都只有一个InputPort
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="allowMulti"></param>
        /// <returns></returns>
        protected Port GenerateInputPort(string portName, bool allowMulti = false)
        {
            Port port = InstantiatePort(Orientation.Horizontal,
                Direction.Input,
                allowMulti? Port.Capacity.Multi : Port.Capacity.Single,
                typeof (bool));
            port.portColor = Color.cyan;
            port.portName = portName;
            this.inputContainer.Add(port);
            this.input = port;
            return port;
        }

        protected Port GenerateOutputPort(string portName, bool allowMulti = false)
        {
            Port port = InstantiatePort(Orientation.Horizontal,
                Direction.Output,
                allowMulti? Port.Capacity.Multi : Port.Capacity.Single,
                typeof (bool));
            port.portColor = Color.cyan;
            port.portName = portName;
            this.outputContainer.Add(port);
            this.outports.TryAdd(portName, port);
            return port;
        }

        protected DialogueNode GetFirstLinkNode(Port output)
        {
            foreach (var edge in output.connections)
            {
                DialogueNodeView nodeView = edge.input.node as DialogueNodeView;
                return nodeView.node;
            }

            return null;
        }

        protected List<DialogueNode> GetLinkNodes(Port output)
        {
            return output.connections
                    .Select(edge => edge.input.node as DialogueNodeView)
                    .Where(nodeView => nodeView != null)
                    .Select(nodeView => nodeView.node)
                    .ToList();
        }

        public abstract void GenerateEdge(DialogueTreeView treeView);

        public virtual void Save(DialogueTreeView treeView)
        {
        }
    }
}