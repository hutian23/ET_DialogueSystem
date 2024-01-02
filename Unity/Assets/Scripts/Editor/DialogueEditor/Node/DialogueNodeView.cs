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
        public TextField TextField;

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

        protected TextField GenerateDescription()
        {
            this.TextField = new TextField();

            // 网上找的，看不懂，反正解决了自动换行的问题
            this.TextField.style.maxWidth = 250;
            this.TextField.style.minWidth = 100;
            this.TextField.style.whiteSpace = WhiteSpace.Normal;
            this.TextField.style.flexDirection = FlexDirection.Row;
            this.TextField.style.flexGrow = 1;
            this.TextField.style.flexWrap = Wrap.Wrap;
            this.TextField.multiline = true;
            this.contentContainer.Add(this.TextField);
            
            this.SaveCallback += this.SaveText;
            this.TextField.value = this.node.text;
            this.TextField.RegisterCallback<ChangeEvent<string>>(this.TextChanged);
            return this.TextField;
        }

        private void TextChanged(ChangeEvent<string> evet)
        {
            Debug.Log("Text Changed");
        }
        
        private void SaveText(DialogueTreeView treeView)
        {
            if (this.TextField == null) return;
            this.node.text = this.TextField.text;
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

        public Action<DialogueTreeView> SaveCallback;
    }
}