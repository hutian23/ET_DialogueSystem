using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            nodeType = type;
        }
    }

    public abstract class DialogueNodeView: Node
    {
        public DialogueNode node;

        public Port input;
        public readonly List<Port> outports = new();
        private TextField TextField;
        protected readonly DialogueTreeView treeView;

        protected DialogueNodeView(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView)
        {
            node = dialogueNode;
            viewDataKey = node.Guid;
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/Resource/NodeView.uss"));
            treeView = dialogueTreeView;
            title = GetNodeTitle();
            SaveCallback += SavePos;
        }

        private string GetNodeTitle()
        {
            NodeTypeAttribute attr = node.GetType().GetCustomAttribute<NodeTypeAttribute>();
            if (attr == null) return "";
            int lastIndex = attr.Level.LastIndexOf('/');

            string id = node.TargetID <= 0? "_" : node.TargetID.ToString();
            return $"[{id}]  {attr.Level.Substring(lastIndex + 1)}";
        }

        private void SavePos()
        {
            node.position.x = GetPosition().xMin;
            node.position.y = GetPosition().yMin;
        }

        protected TextField GenerateDescription()
        {
            TextField = new TextField();

            // 网上找的，看不懂，反正解决了自动换行的问题
            TextField.style.maxWidth = 250;
            TextField.style.minWidth = 100;
            TextField.style.whiteSpace = WhiteSpace.Normal;
            TextField.style.flexDirection = FlexDirection.Row;
            TextField.style.flexGrow = 1;
            TextField.style.flexWrap = Wrap.Wrap;
            TextField.multiline = true;
            contentContainer.Add(TextField);

            SaveCallback += () => { node.text = TextField.text; };
            TextField.RegisterCallback<BlurEvent>(_ => treeView.SetDirty());

            TextField.value = node.text;
            return TextField;
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
            inputContainer.Add(port);
            input = port;
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
            outputContainer.Add(port);
            outports.Add(port);
            return port;
        }

        protected int GetFirstLinkNode(Port output)
        {
            foreach (var edge in output.connections)
            {
                DialogueNodeView nodeView = edge.input.node as DialogueNodeView;
                return nodeView.node.TargetID;
            }

            return -1;
        }

        /// <summary>
        /// 返回相连节点的TargetID
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        protected List<int> GetLinkNodes(Port output)
        {
            return output.connections
                    .Select(edge => edge.input.node as DialogueNodeView)
                    .Where(nodeView => nodeView != null)
                    .Select(nodeView => nodeView.node)
                    .Select(dialogueNode => dialogueNode.TargetID)
                    .ToList();
        }
        
        public Action SaveCallback;

        public DialogueNode Clone()
        {
            return this.node.Clone();
        }
    }
}