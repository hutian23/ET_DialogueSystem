using System;
using ET.Client;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public abstract class ParamResolver
    {
    }

    public sealed class ParamResolver<T, K>: ParamResolver where T : BaseField<K>
    {
        private readonly T editorField;
        private readonly Label label;

        private readonly BehaviorControllerEditor controllerEditor;
        private readonly SharedVariable variable;

        public ParamResolver(SharedVariable _variable, BehaviorControllerEditor _editor)
        {
            variable = _variable;
            controllerEditor = _editor;

            var paramView = new BehaviorParamView();
            controllerEditor.parameterViewContainer.Add(paramView);

            //Editor Field
            paramView.style.flexDirection = FlexDirection.Row;
            editorField = Activator.CreateInstance(GetType().GenericTypeArguments[0]) as T;
            editorField.SetValueWithoutNotify((K)variable.value);
            editorField.RegisterCallback<BlurEvent>(_ => { Save(); });

            editorField.style.width = 100;
            editorField.style.height = 23;
            editorField.style.right = 6;

            //Label
            label = new Label(variable.name);
            label.style.position = Position.Absolute;
            label.style.left = 5;
            label.style.top = 7;
            label.style.width = 100;
            label.text = variable.name;

            paramView.Add(label);
            paramView.Add(editorField);
        }

        private void Save()
        {
            controllerEditor.ApplyModify(() =>
            {
                variable.name = label.text;
                variable.value = editorField.value;
            }, "Save value");
        }
    }
}