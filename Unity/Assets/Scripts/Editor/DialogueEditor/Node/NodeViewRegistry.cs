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
    public static class NodeViewRegistry
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
            var types = AssemblyHelper.GetAssemblyTypes(typeof (NodeViewRegistry).Assembly);
            foreach (var type in types.Values)
            {
                if (type.IsGenericType || type.IsAbstract) continue;
                if (type.IsSubclassOf(typeof (DialogueNodeView)))
                {
                    NodeClassMap.TryAdd(type.BaseType.GenericTypeArguments[0], type);
                }
            }
        }
    }
}