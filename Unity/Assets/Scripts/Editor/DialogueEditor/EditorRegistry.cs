using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public static class EditorRegistry
    {
        private static readonly Dictionary<Type, Type> NodeClassMap = new();
        public static readonly Dictionary<Type, Type> resolverMap = new();

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

            EditorApplication.delayCall += RegisterEditorType;
        }

        private static void RegisterResolver(Type variableType, Type resolveType)
        {
            if (resolverMap.TryGetValue(variableType, out Type oldResolverType))
            {
                Debug.LogError($"{variableType}已存在resolver {oldResolverType}");
                return;
            }

            resolverMap.Add(variableType, resolveType);
        }

        private static void RegisterEditorType()
        {
            //注册NodeView映射
            NodeClassMap.Clear();
            var types = AssemblyHelper.GetAssemblyTypes(typeof (EditorRegistry).Assembly);
            foreach (var type in types.Values)
            {
                if (type.IsGenericType || type.IsAbstract) continue;
                if (type.IsSubclassOf(typeof (DialogueNodeView)))
                {
                    NodeClassMap.TryAdd(type.BaseType.GenericTypeArguments[0], type);
                }
            }

            //注册resolver
            resolverMap.Clear();
            RegisterResolver(typeof (int), typeof (FieldResolver<IntegerField, int>));
            RegisterResolver(typeof (float), typeof (FieldResolver<FloatField, float>));
            RegisterResolver(typeof (bool), typeof (FieldResolver<Toggle, bool>));
            RegisterResolver(typeof (String), typeof (FieldResolver<TextField, String>));
            RegisterResolver(typeof (Vector2), typeof (FieldResolver<Vector2Field, Vector2>));
            RegisterResolver(typeof (Vector3), typeof (FieldResolver<Vector3Field, Vector3>));
            RegisterResolver(typeof (AnimationCurve), typeof (FieldResolver<CurveField, AnimationCurve>));
        }
    }
}