using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class DialogueBlackboard: Blackboard
    {
        private DialogueTreeView treeView;
        public readonly ScrollView RawContainer;

        public DialogueBlackboard(GraphView _graphView): base(_graphView)
        {
            var header = this.Q("header");
            header.style.minHeight = new StyleLength(40);
            Add(RawContainer = new ScrollView());
        }

        public void PopulateView(DialogueTreeView _graphView)
        {
            RawContainer.Clear();
            treeView = _graphView;

            addItemRequested = _ =>
            {
                GenericMenu menu = new();
                EditorRegistry.resolverMap.ForEach(kv =>
                {
                    menu.AddItem(new GUIContent(kv.Key.Name), false, () =>
                    {
                        object obj = kv.Key == typeof (string)? "" : Activator.CreateInstance(kv.Key);
                        AddVariable(obj);
                    });
                });
                menu.ShowAsContext();
            };

            editTextRequested += (_, element, newValue) =>
            {
                //检查同名属性
                if (ContainVariable(newValue)) return;
                ((BlackboardField)element).text = newValue;
                treeView.SetDirty();
            };

            treeView.GetTree().Variables.ForEach(v =>
            {
                FieldResolver resolver = Activator.CreateInstance(EditorRegistry.resolverMap[v.value.GetType()], args: new object[] { v, treeView }) as FieldResolver;
                RawContainer.Add(resolver.CreateRow());
            });
        }

        private void AddVariable(object obj)
        {
            string variableName = "Constant";
            //检查重名
            int id = 0;
            while (ContainVariable(variableName))
            {
                variableName = $"Constant({++id})";
            }

            SharedVariable variable = new() { name = variableName, value = obj };
            treeView.GetTree().Variables.Add(variable);

            FieldResolver resolver = Activator.CreateInstance(EditorRegistry.resolverMap[obj.GetType()], args: new object[] { variable, treeView }) as FieldResolver;
            RawContainer.Add(resolver.CreateRow());
        }

        private bool ContainVariable(string variableName)
        {
            return treeView.GetTree().Variables.Any(v => v.name == variableName);
        }

        public void Save()
        {
            var variables = treeView.GetTree().Variables;
            variables.Clear();

            RawContainer.Children().Cast<BlackboardRow>().ForEach(row =>
            {
                FieldResolver resolver = row.Q<RefernceElement>().reference as FieldResolver;
                resolver.Save();
                variables.Add(resolver.Variable);
            });
        }
    }
}