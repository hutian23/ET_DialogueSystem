using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public abstract class FieldResolver
    {
        protected SharedVariable Variable;
        protected DialogueTreeView treeView;

        protected FieldResolver(SharedVariable variable, DialogueTreeView _treeView)
        {
            Variable = variable;
            treeView = _treeView;
        }

        public abstract BlackboardRow Create();
        public abstract void Save();
    }

    public class TagElement: VisualElement
    {
        public int index;
    }

    public class FieldResolver<T, K>: FieldResolver where T : BaseField<K>
    {
        private T editorField;
        private BlackboardRow row;

        public FieldResolver(SharedVariable variable, DialogueTreeView _treeView): base(variable, _treeView)
        {
        }

        public override BlackboardRow Create()
        {
            var blackboardField = new BlackboardField() { text = this.Variable.name, typeText = Variable.value.GetType().Name };
            blackboardField.capabilities &= ~ Capabilities.Deletable;
            blackboardField.capabilities &= ~ Capabilities.Movable;

            editorField = Activator.CreateInstance(GetType().GenericTypeArguments[0]) as T;
            editorField.SetValueWithoutNotify((K)Variable.value);
            editorField.RegisterValueChangedCallback(_ => { treeView.SetDirty(); });

            row = new BlackboardRow(blackboardField, editorField);
            this.row.Add(new TagElement() { index = 1 });
            row.AddManipulator(new ContextualMenuManipulator(evt => BuildBlackboardMenu(evt, row)));
            return row;
        }

        private void BuildBlackboardMenu(ContextualMenuPopulateEvent evt, VisualElement ele)
        {
            evt.menu.MenuItems().Clear();
            evt.StopPropagation(); // 避免调用到父级treeview的BuildContextualMenu()
            evt.menu.AppendAction("编辑", _ => { ele.Q<BlackboardField>().OpenTextEditor(); });
            evt.menu.AppendAction("移除", _ =>
            {
                treeView.GetBlackboard().RawContainer.Remove(ele);
                Debug.Log(ele.Q<TagElement>().index);
            });
        }

        public override void Save()
        {
            Variable.name = row.Q<BlackboardField>().text;
            Variable.value = editorField.value;
        }
    }
}