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
    public static class NodeEditorRegistry
    {
        private static readonly Dictionary<Type, Type> NodeClassMap = new();
        
        public static Type LookUpNodeEditor(Type type)
        {
            if (NodeClassMap.TryGetValue(type, out Type editorType))
            {
                return editorType;
            }
            Debug.LogError($"not found editorType of{type}");
            return null;
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CreateAssetWhenReady()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += CreateAssetWhenReady;
                return;
            }
            
            EditorApplication.delayCall += RegisterNodeEditor;
        }

        private static void RegisterNodeEditor()
        {
            NodeClassMap.Clear();

            var types = AssemblyHelper.GetAssemblyTypes(typeof (NodeEditorRegistry).Assembly);
            foreach (var type in types.Values)
            {
                if (type.IsGenericType || type.IsAbstract) continue;
                if (type.GetCustomAttribute(typeof (NodeViewAttribute)) != null)
                {
                    //节点和节点视图的映射关系
                    NodeClassMap.TryAdd(type.BaseType.GenericTypeArguments[0], type);
                }
            }
        }
    }

    public class NodeViewAttribute: Attribute
    {
    }

    [NodeView]
    public abstract class NodeEditorBase<T> : Node where T : DialogueNode
    {
        public Type NominalType
        {
            get => typeof (T);
        }

        public void SetTarget(DialogueNode dialogueNode)
        {
            if (node.GetType() == NominalType) node = dialogueNode as T;
        }

        protected T node;

        public Port input;
        public readonly List<Port> outports = new();
        private TextField TextField;
        protected DialogueTreeView treeView;

        public NodeEditorBase(DialogueNode dialogueNode, DialogueTreeView dialogueTreeView)
        {
            if(node.GetType() != this.NominalType) Debug.LogError($"{node} is cannot convert to {NominalType}");
            node = dialogueNode as T;
            viewDataKey = node.Guid;
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/DialogueEditor/Resource/NodeView.uss"));
            treeView = dialogueTreeView;
            title = GetNodeTitle();
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
        
        protected uint GetFirstLinkNode(Port output)
        {
            foreach (var edge in output.connections)
            {
                DialogueNodeView nodeView = edge.input.node as DialogueNodeView;
                return nodeView.node.TargetID;
            }

            return 0;
        }
        
        
        /// <summary>
        /// 返回相连节点的TargetID
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        protected List<uint> GetLinkNodes(Port output)
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
            return node.Clone();
        }
    }
}