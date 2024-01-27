using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ET.Client
{
    public abstract class FieldResolver
    {
        public SharedVariable Variable;
        protected DialogueTreeView treeView;

        protected FieldResolver(SharedVariable variable, DialogueTreeView _treeView)
        {
            Variable = variable;
            treeView = _treeView;
        }

        public abstract BlackboardRow CreateRow();
        public abstract void Save();
    }

    public class RefernceElement: VisualElement
    {
        public object reference;
    }

    public sealed class FieldResolver<T, K>: FieldResolver where T : BaseField<K>
    {
        private T editorField;
        private BlackboardRow row;

        public FieldResolver(SharedVariable variable, DialogueTreeView _treeView): base(variable, _treeView)
        {
        }

        public override BlackboardRow CreateRow()
        {
            var blackboardField = new BlackboardField() { text = this.Variable.name, typeText = Variable.value.GetType().Name };
            blackboardField.capabilities &= ~ Capabilities.Deletable;
            blackboardField.capabilities &= ~ Capabilities.Movable;

            editorField = Activator.CreateInstance(GetType().GenericTypeArguments[0]) as T;
            editorField.SetValueWithoutNotify((K)Variable.value);
            editorField.RegisterValueChangedCallback(_ => { treeView.SetDirty(); });

            row = new BlackboardRow(blackboardField, editorField);
            row.Add(new RefernceElement() { reference = this });
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
                var resolver = ele.Q<RefernceElement>().reference as FieldResolver;
                treeView.RemoveCaches.Add(resolver.Variable);
                treeView.GetBlackboard().RawContainer.Remove(ele);
            });
        }

        public override void Save()
        {
            Variable.name = row.Q<BlackboardField>().text;
            Variable.value = editorField.value;
        }
    }
}