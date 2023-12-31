using System;
using System.Collections.Generic;
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
    
    public class DialogueNodeView : Node
    {
        protected DialogueNode node;
        protected Port inputPort;
        protected List<Port> outputPorts;

        protected Action<DialogueNodeView> OnNodeSelected;
        
        public DialogueNodeView(DialogueNode node)
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

        public virtual void GeneratePort()
        {
            
        }
        
        protected Label GenerateDescription()
        {
            Label desc = new();
            desc.enableRichText = true;
            this.inputContainer.Add(desc);
            desc.AddToClassList("input-port-label");
            this.inputContainer.style.flexDirection = FlexDirection.Row;
            return desc;
        }
        
        protected Port GenerateInputPort(string portName, bool allowMulti = false)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, allowMulti? Port.Capacity.Multi : Port.Capacity.Single, typeof (bool));
            port.portColor = Color.cyan;
            port.portName = portName;
            this.inputContainer.Add(port);
            return port;
        }
        
        protected Port GenerateOutputPort(string portName,bool allowMulti = false)
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, allowMulti? Port.Capacity.Multi : Port.Capacity.Single, typeof (bool));
            port.portColor = Color.cyan;
            port.portName = portName;
            this.outputContainer.Add(port);
            return port;
        }
    }
}